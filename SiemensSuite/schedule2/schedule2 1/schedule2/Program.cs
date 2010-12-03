using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.IO; 

namespace schedule2
{   
    class Program
    {
        private const int MAXPRIO = 3;
        private const int MAXLOPRIO = 2;
        private const int BLOCKPRIO = 0;
        private const int CMDSIZE = 20; /* size of command buffer */

        /* Scheduling commands */
        private const int NEW_JOB = 1;
        private const int UPGRADE_PRIO = 2;
        private const int BLOCK = 3;
        private const int UNBLOCK = 4;
        private const int QUANTUM_EXPIRE = 5;
        private const int FINISH = 6;
        private const int FLUSH = 7;

        /* stati */
        private const int OK = 0;
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int BADNOARGS = -1; /* Wrong number of arguments */
        private const int BADARG = -2;    /* Bad argument (< 0) */
        private const int MALLOC_ERR = -3;
        private const int BADPRIO = -4;   /* priority < 0 or > MAXPRIO */
        private const int BADRATIO = -5;  /* ratio < 0 or > 1 */
        private const int NO_COMMAND = -6; /* No such scheduling command */

        static Process current_job;
        static int next_pid = 0;
        static LinkedList<Process>[] prio_queue = new LinkedList<Process>[MAXPRIO + 1]; /* blocked queue is [0] */
        
        public static int enqueue(int prio, Process new_process)
        {
            if (prio_queue[prio] == null)
                prio_queue[prio] = new LinkedList<Process>();

            int status = put_end(prio, new_process);
            if(status != 0) 
                return(status); /* Error */

            return(reschedule(prio));
        }

        public static int put_end(int prio, Process process) /* Put process at end of queue */
        {
            if(prio > MAXPRIO || prio < 0) 
                return(BADPRIO); /* Somebody goofed */

            prio_queue[prio].AddLast(process);
            return(OK);
        }

        public static int reschedule(int prio) /* Put highest priority job into current_job */
        {
            if(current_job != null && prio > current_job.priority)
            {
	            put_end(current_job.priority, current_job);
	            current_job = null;
            }
            get_current(); /* Reschedule */
            return(OK);
        }

        public static Process get_current() /* If no current process, get it. Return it */
        {
            int prio;
            if(current_job == null)
            {
	            for(prio = MAXPRIO; prio > 0; prio--)
	            { /* find head of highest queue with a process */
	                if(get_process(prio, (float)0.0, ref current_job) > 0) 
                        break;
	            }
            }
            return(current_job);
        }


        public static int get_process(int prio, float ratio, ref Process job)
        {
            int length, index;
            //struct process **next;
            LinkedListNode<Process> next;

            if(prio > MAXPRIO || prio < 0) 
                return(BADPRIO); /* Somebody goofed */
            if(ratio < 0.0 || ratio > 1.0) 
                return(BADRATIO); /* Somebody else goofed */

            length = prio_queue[prio].Count;
            index = (int)ratio * length;
            index = index >= length ? length -1 : index; /* If ratio == 1.0 */


            for(next = prio_queue[prio].First; index>0 && next.Next != null; index--) {
                if(index > 0)
                    next = next.Next; /* Count up to it */
            }

            if (next == null)
            {
                job = null;
            }
            else
            {
                job = next.Value;
            }

            
            if(job != null)
            {
	            prio_queue[prio].Remove(next);
	            return(TRUE);
            }
            else 
                return(FALSE);
        }

        public static int new_job(int prio) /* allocate new pid and process block. Stick at end */
        {
            int pid, status = OK;
            Process new_process;
            pid = next_pid++;
            new_process = new Process();
            if(new_process == null) 
                status = MALLOC_ERR;
            else
            {
	            new_process.pid = pid;
	            new_process.priority = prio;                
	            status = enqueue(prio, new_process);
            }
            if(status != 0) 
                next_pid--; /* Unsuccess. Restore pid */
            return(status);
        }

        public static int upgrade_prio(int prio, float ratio) /* increment priority at ratio in queue */
        {
            int status;
            Process job = null;
        /*    if(prio < 1 || prio > MAXLOPRIO) return(BADPRIO); MISSING CODE */
            if((status = get_process(prio, ratio, ref job)) <= 0) 
                return(status);
            /* We found a job in that queue. Upgrade it */
            job.priority = prio + 1;

            return(enqueue(prio + 1, job));
        }

        public static int block() /* Put current job in blocked queue */
        {
            Process job;
            job = get_current();
            if(job != null)
            {
	            current_job = null; /* remove it */
	            return(enqueue(BLOCKPRIO, job)); /* put into blocked queue */
            }
            return(OK);
        }

        public static int unblock(float ratio) /* Restore job @ ratio in blocked queue to its queue */
        {
            int status;
            Process job = null;
            if((status = get_process(BLOCKPRIO, ratio, ref job)) <= 0) 
                return(status);

            /* We found a blocked process. Put it where it belongs. */
            return(enqueue(job.priority, job));
        }

        public static int quantum_expire() /* put current job at end of its queue */
        {
            Process job;
            job = get_current();
            if(job != null)
            {
	            current_job = null; /* remove it */
	            return(enqueue(job.priority, job));
            }
            return(OK);
        }



        public static int finish() /* Get current job, print it, and zap it. */
        {
            Process job;
            job = get_current();
            if(job != null)
            {
	            current_job = null;
	            reschedule(0);
	            //fprintf(stdout, " %d", job->pid);
                System.Console.Write(" {0}", job.pid);
	            //free(job);
	            return(FALSE);
            }
            else return(TRUE);
        }

        public static int flush() /* Get all jobs in priority queues & zap them */
        {
            int m;
            while(finish() == 0)
                m=0;
            //fprintf(stdout, "\n");
            System.Console.WriteLine("");
            return(OK);
        }

        public static int schedule(int command, int prio, float ratio)
        {
            int status = OK;
            switch (command)
            {
                case NEW_JOB:
                    status = new_job(prio);
                    break;
                case QUANTUM_EXPIRE:
                    status = quantum_expire();
                    break;
                case UPGRADE_PRIO:
                    status = upgrade_prio(prio, ratio);
                    break;
                case BLOCK:
                    status = block();
                    break;
                case UNBLOCK:
                    status = unblock(ratio);
                    break;
                case FINISH:
                    finish();
                    //fprintf(stdout, "\n");
                    System.Console.WriteLine("");
                    break;
                case FLUSH:
                    status = flush();
                    break;
                default:
                    status = NO_COMMAND;
                    break;
            }
            return (status);
        }

        //main(int argc, char[] argv) /* n3, n2, n1 : # of processes at prio3 ... */
        //{
        //    int command, prio;
        //    float ratio;
        //    int nprocs, status, pid;
        //    struct process *process;
        //    if(argc != MAXPRIO + 1) exit_here(BADNOARGS);
        //    for(prio = MAXPRIO; prio > 0; prio--)
        //    {
        //    if((nprocs = atoi(argv[MAXPRIO + 1 - prio])) < 0) exit_here(BADARG);
        //    for(; nprocs > 0; nprocs--)
        //    {
        //        if(status = new_job(prio)) exit_here(status);
        //    }
        //    }
        //    /* while there are commands, schedule it */
        //    while((status = get_command(&command, &prio, &ratio)) > 0)
        //    {
        //    schedule(command, prio, ratio);
        //    }
        //    if(status < 0) exit_here(status); /* Real bad error */
        //    exit_here(OK);
        //}

        //public static int get_command()
        //{
        //    int status = OK;
        //    char buf[CMDSIZE];
        //    if(fgets(buf, CMDSIZE, stdin))
        //    {
        //        f_prio = -1;
        //        f_command = -1; 
        //        f_ratio =(float)-1.0;

        //        sscanf(buf, "%d", command);
        //        switch(*command)
        //        {
        //          case NEW_JOB :
        //            sscanf(buf, "%*s%d", prio);
        //            break;
        //          case UNBLOCK :
        //            sscanf(buf, "%*s%f", ratio);
        //            break;
        //          case UPGRADE_PRIO :
        //            sscanf(buf, "%*s%d%f", prio, ratio);
        //            break;
        //        }
        //         /* Find end of  line of input if no EOF */
        //        while(buf[strlen(buf)-1] != '\n' && fgets(buf, CMDSIZE, stdin));
        //        return(TRUE);
        //    }
        //    else return(FALSE);
        //}

        //exit_here(status)
        //     int status;
        //{
        //    exit(abs(status));
        //}


        public static ArrayList readFile(string path)
        {
            string line;
            StreamReader file = new StreamReader(path);
            ArrayList fileContents = new ArrayList();
            while ((line = file.ReadLine()) != null)
            {
                line = line.Trim();
                fileContents.Add(line);
            }

            return fileContents;
        }
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int command, prio;
            float ratio;
            int nprocs, status, pid;
            Process process;
            string[] lineSeq;

            if(args.Length != MAXPRIO + 1) 
            {
                System.Console.WriteLine("BADNOARGS");
                return;
            }
            for(prio = MAXPRIO; prio > 0; prio--)
            {
                nprocs = Int32.Parse(args[MAXPRIO - prio]);
                if (nprocs < 0)
                {
                    System.Console.WriteLine("BADARG");
                    return;
                }
                for(; nprocs > 0; nprocs--)
                {
                    status = new_job(prio);
                    if (status ==1)
                    {
                        System.Console.WriteLine("{0}", status);
                        return;
                    }
                }
            }

            ArrayList test = readFile(args[MAXPRIO]);

            foreach (string s in test)
            {
                prio = -1;
                command = -1;
                ratio = (float)-1.0;

                lineSeq = s.Split(' ');
                //status = lineSeq[0].Equals("");
                //if (status)
                //    continue;
                command = Int32.Parse(lineSeq[0]);

                switch (command)
                {
                    case NEW_JOB:
                        //sscanf(buf, "%*s%d", prio);
                        if (lineSeq.Length != 2)
                        {
                            System.Console.WriteLine("bad arguments");
                            return;
                        }
                        prio = Int32.Parse(lineSeq[1]);
                        break;
                    case UNBLOCK:
                        //sscanf(buf, "%*s%f", ratio);
                        if (lineSeq.Length != 2)
                        {
                            System.Console.WriteLine("bad arguments");
                            return;
                        }
                        ratio = (float)Double.Parse(lineSeq[1]);
                        break;
                    case UPGRADE_PRIO:
                        //sscanf(buf, "%*s%d%f", prio, ratio);
                        if (lineSeq.Length != 3)
                        {
                            System.Console.WriteLine("bad arguments");
                            return;
                        }
                        prio = Int32.Parse(lineSeq[1]);
                        ratio = (float)Double.Parse(lineSeq[2]);
                        break;
                }

                schedule(command, prio, ratio);
            }

            /* while there are commands, schedule it */
            //while((status = get_command()) > 0)
            //{
            //    schedule(command, prio, ratio);
            //}

            //if (status < 0) /* Real bad error */
            //{
            //    System.Console.WriteLine("{0}", status);
            //    return;
            //}

            //exit_here(OK);
        }
    }
}
