using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.IO; 


namespace schedule
{
    public class Program
    {
        private const int NULL = 0;
        private const int NEW_JOB = 1;
        private const int UPGRADE_PRIO = 2;
        private const int BLOCK = 3;
        private const int UNBLOCK = 4;
        private const int QUANTUM_EXPIRE = 5;
        private const int FINISH = 6;
        private const int FLUSH = 7;

        public const int MAXPRIO = 3;

        private static int alloc_proc_num;
        private static int num_processes;
        private static LinkedListNode<Ele> cur_proc;
        public static LinkedList<Ele>[] prio_queue = new LinkedList<Ele>[MAXPRIO + 1]; 	/* 0th element unused */
        private static LinkedList<Ele> block_queue;

        /*-----------------------------------------------------------------------------
          new_list
                allocates, initializes and returns a new list.
                Note that if the argument compare() is provided, this list can be
                    made into an ordered list. see insert_ele().
        -----------------------------------------------------------------------------*/
        public static LinkedList<Ele> new_list()
        {
            LinkedList<Ele> List = new LinkedList<Ele>();
            return List;
        }

        /*-----------------------------------------------------------------------------
          append_ele
                appends the new_ele to the list. If list is null, a new
            list is created. The modified list is returned.
        -----------------------------------------------------------------------------*/
        public static LinkedList<Ele> append_ele(LinkedList<Ele> a_list, Ele a_ele)
        {
          if (a_list == null)
              a_list = new_list();	/* make list without compare function */

          a_list.AddLast(a_ele); /* insert at the tail */
          return (a_list);
        }

        /*-----------------------------------------------------------------------------
          find_nth
                fetches the nth element of the list (count starts at 1)
        -----------------------------------------------------------------------------*/
        public static LinkedListNode<Ele> find_nth(LinkedList<Ele> f_list, int n)
        {
            LinkedListNode<Ele> f_ele;
            int i;

            if (f_list == null)
                return null;

            f_ele = f_list.First;
            for (i=0; (f_ele != null) && (i<(n-1)); i++) { /* logic error */ 
                f_ele = f_ele.Next;
            }
            
            return f_ele;
        }

        /*-----------------------------------------------------------------------------
          del_ele
                deletes the old_ele from the list.
                Note: even if list becomes empty after deletion, the list
                  node is not deallocated.
        -----------------------------------------------------------------------------*/
        public static LinkedList<Ele> del_ele(LinkedList<Ele> d_list, LinkedListNode<Ele> d_ele)
        {
            if (d_list == null || d_ele == null)
                return (null);
            
            d_list.Remove(d_ele);
            return (d_list);
        }

        /*-----------------------------------------------------------------------------
           free_ele
               deallocate the ptr. Caution: The ptr should point to an object
               allocated in a single call to malloc.
        -----------------------------------------------------------------------------*/
        //void free_ele(ptr)
        //Ele *ptr;
        //{
        //    free(ptr);
        //}

        public static void finish_process()
        {
            schedule();
            if (cur_proc != null)
            {
                //fprintf(stdout, "%d ", cur_proc->val);
                Console.WriteLine("{0}", cur_proc.Value.val);
                //free_ele(cur_proc);
                num_processes--;
            }
        }

        public static void finish_all_processes()
        {
            int i;
            int total;
            total = num_processes;
            for (i=0; i<total; i++) {
                finish_process();
            }
        }

        public static void schedule()
        {
            int i;
            
            cur_proc = null;
            for (i=MAXPRIO; i > 0; i--)
            {
                if (prio_queue[i].Count > 0)
                {
                    cur_proc = prio_queue[i].First;
                    prio_queue[i] = del_ele(prio_queue[i], cur_proc);
                    return;
                }
            }
        }

        public static void upgrade_process_prio(int prio, float ratio)
        {
            int count;
            int n;
            LinkedListNode<Ele> procN;
            Ele proc;
            LinkedList<Ele> src_queue; 
            LinkedList<Ele> dest_queue;
            
            if (prio >= MAXPRIO)
                return;
            src_queue = prio_queue[prio];
            dest_queue = prio_queue[prio+1];
            count = src_queue.Count;

            if (count > 0)
            {
                n = (int) (count*ratio + 1);
                procN = find_nth(src_queue, n);
                if (procN != null) {
                    proc = procN.Value;
                    src_queue = del_ele(src_queue, procN);
                    /* append to appropriate prio queue */
                    proc.priority = prio;
                    dest_queue = append_ele(dest_queue, proc);
                }
            }
        }

        public static void unblock_process(float ratio)
        {
            int count;
            int n;
            LinkedListNode<Ele> procN;
            Ele proc;
            int prio;
            if (block_queue != null)
            {
                count = block_queue.Count;
                n = (int) (count*ratio + 1);
                procN = find_nth(block_queue, n);
                if (procN != null) {
                    proc = procN.Value;
                    block_queue = del_ele(block_queue, procN);
                    /* append to appropriate prio queue */
                    prio = proc.priority;
                    prio_queue[prio] = append_ele(prio_queue[prio], proc);
                }
            }
        }

        public static void quantum_expire()
        {
            int prio;
            schedule();
            if (cur_proc != null)
            {
                prio = cur_proc.Value.priority;
                prio_queue[prio] = append_ele(prio_queue[prio], cur_proc.Value);
            }	
        }
        	
        public static void block_process()
        {
            schedule();
            if (cur_proc != null)
            {
                block_queue = append_ele(block_queue, cur_proc.Value);
            }
        }

        public static Ele new_process(int prio)
        {
            Ele proc;
            proc = new Ele(alloc_proc_num++);
            proc.priority = prio;
            num_processes++;
            return proc;
        }

        public static void addProcess(int prio)
        {
            Ele proc;
            proc = new_process(prio);
            prio_queue[prio] = append_ele(prio_queue[prio], proc);
        }

        public static void init_prio_queue(int prio, int num_proc)
        {
            LinkedList<Ele> queue;
            Ele proc;
            int i;
            
            queue = new_list();
            for (i=0; i<num_proc; i++)
            {
                proc = new_process(prio);
                queue = append_ele(queue, proc);
            }
            prio_queue[prio] = queue;
        }

        public static void initialize()
        {
            alloc_proc_num = 0;
            num_processes = 0;
        }

        public static ArrayList readFile(string path)
        {
            /*string line;
            StreamReader file = new StreamReader(path);
            ArrayList fileContents = new ArrayList();
            while ((line = file.ReadLine()) != null)
            {
                line = line.Trim();
                if (!line.Equals(""))
                {
                    fileContents.Add(line);
                }
            }
			*/
            ArrayList fileContents = new ArrayList();
            fileContents.AddRange(path.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
            return fileContents;
        }
        
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            int command;
            int prio;
            float ratio;
            bool status;
            string[] lineSeq;

            if (args.Length < (MAXPRIO))
            {
                System.Console.WriteLine("incorrect usage. %d", args.Length);
                return;
            }

            initialize();
            for (prio = MAXPRIO; prio > 0; prio--)
            {
                init_prio_queue(prio, Int32.Parse(args[prio-1]));
            }

            ArrayList test = readFile(args[MAXPRIO]);

            foreach (string s in test)
            { 
                lineSeq = s.Split(' ');
                //status = lineSeq[0].Equals("");
                //if (status)
                //    continue;
                command = Int32.Parse(lineSeq[0]);

                switch (command)
                {
                    case FINISH:
                        finish_process();
                        break;
                    case BLOCK:
                        block_process();
                        break;
                    case QUANTUM_EXPIRE:
                        quantum_expire();
                        break;
                    case UNBLOCK:
                        if (lineSeq.Length != 2)
                        {
                            System.Console.WriteLine("bad arguments");
                            return;
                        }
                        //fscanf(stdin, "%f", &ratio);
                        ratio = (float)Double.Parse(lineSeq[1]);
                        unblock_process(ratio);
                        break;
                    case UPGRADE_PRIO:
                        if (lineSeq.Length != 3)
                        {
                            System.Console.WriteLine("bad arguments");
                            return;
                        }
                        //fscanf(stdin, "%d", &prio);
                        //fscanf(stdin, "%f", &ratio);
                        prio = Int32.Parse(lineSeq[1]);
                        ratio = (float)Double.Parse(lineSeq[2]);
                        if (prio > MAXPRIO || prio <= 0)
                        {
                            System.Console.WriteLine("** invalid priority\n");
                            return;
                        }
                        else
                            upgrade_process_prio(prio, ratio);
                        break;
                    case NEW_JOB:
                        if (lineSeq.Length != 2)
                        {
                            System.Console.WriteLine("bad arguments");
                            return;
                        }
                        //fscanf(stdin, "%d", &prio);
                        prio = Int32.Parse(lineSeq[1]);
                        if (prio > MAXPRIO || prio <= 0)
                        {
                            System.Console.WriteLine("** invalid priority\n");
                            return;
                        }
                        else
                            addProcess(prio);
                        break;
                    case FLUSH:
                        finish_all_processes();
                        break;
                }
                
            }

        }
    }
}
