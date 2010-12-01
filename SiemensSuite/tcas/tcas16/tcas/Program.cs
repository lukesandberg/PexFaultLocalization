using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace tcas
{
    class Program
    {
        private const int OLEV = 600;		/* in feets/minute */
        private const int MAXALTDIFF = 600;		/* max altitude difference in feet */
        private const int MINSEP = 300;          /* min separation in feet */
        private const int NOZCROSS = 100;		/* in feet */
				        /* variables */

        static int Cur_Vertical_Sep;
        static bool High_Confidence;
        static bool Two_of_Three_Reports_Valid;

        static int Own_Tracked_Alt;
        static int Own_Tracked_Alt_Rate;
        static int Other_Tracked_Alt;

        static int Alt_Layer_Value;		/* 0, 1, 2, 3 */
        static int[] Positive_RA_Alt_Thresh = new int[4];

        static int Up_Separation;
        static int Down_Separation;

				        /* state variables */
        static int Other_RAC;			/* NO_INTENT, DO_NOT_CLIMB, DO_NOT_DESCEND */
        private const int  NO_INTENT = 0;
        private const int  DO_NOT_CLIMB = 1;
        private const int  DO_NOT_DESCEND = 2;

        static int Other_Capability;		/* TCAS_TA, OTHER */
        private const int  TCAS_TA = 1;
        private const int  OTHER = 2;

        static int Climb_Inhibit;		/* true/false */

        private const int  UNRESOLVED = 0;
        private const int  UPWARD_RA = 1;
        private const int  DOWNWARD_RA = 2;


        public static void initialize()
        {
            Positive_RA_Alt_Thresh[0] = 400 + 1;
            Positive_RA_Alt_Thresh[1] = 500;
            Positive_RA_Alt_Thresh[2] = 640;
            Positive_RA_Alt_Thresh[3] = 740;
        }

        public static int ALIM()
        {
            return Positive_RA_Alt_Thresh[Alt_Layer_Value];
        }

        public static int Inhibit_Biased_Climb()
        {
            //return (Climb_Inhibit ? Up_Separation + NOZCROSS : Up_Separation);
            if (Climb_Inhibit == 1)
                return Up_Separation + NOZCROSS;
            else
                return Up_Separation;
        }

        public static bool Non_Crossing_Biased_Climb()
        {
            bool upward_preferred;
            int upward_crossing_situation;
            bool result;

            upward_preferred = Inhibit_Biased_Climb() > Down_Separation;
            if (upward_preferred)
            {
                result = !(Own_Below_Threat()) || ((Own_Below_Threat()) && (!(Down_Separation >= ALIM()))); /* opertor mutation */
            }
            else
            {
                result = Own_Above_Threat() && (Cur_Vertical_Sep >= MINSEP) && (Up_Separation >= ALIM());
            }
            return result;
        }

        public static bool Non_Crossing_Biased_Descend()
        {
            bool upward_preferred;
            int upward_crossing_situation;
            bool result;

            upward_preferred = Inhibit_Biased_Climb() > Down_Separation;
            if (upward_preferred)
            {
                result = Own_Below_Threat() && (Cur_Vertical_Sep >= MINSEP) && (Down_Separation >= ALIM());
            }
            else
            {
                result = !(Own_Above_Threat()) || ((Own_Above_Threat()) && (Up_Separation >= ALIM()));
            }
            return result;
        }

        public static bool Own_Below_Threat()
        {
            return (Own_Tracked_Alt < Other_Tracked_Alt);
        }

        public static bool Own_Above_Threat()
        {
            return (Other_Tracked_Alt < Own_Tracked_Alt);
        }

        public static int alt_sep_test()
        {
            bool enabled, tcas_equipped, intent_not_known;
            bool need_upward_RA, need_downward_RA;
            int alt_sep;

            enabled = High_Confidence && (Own_Tracked_Alt_Rate <= OLEV) && (Cur_Vertical_Sep > MAXALTDIFF);
            tcas_equipped = Other_Capability == TCAS_TA;
            intent_not_known = Two_of_Three_Reports_Valid && Other_RAC == NO_INTENT;

            alt_sep = UNRESOLVED;

            if (enabled && ((tcas_equipped && intent_not_known) || !tcas_equipped))
            {
                need_upward_RA = Non_Crossing_Biased_Climb() && Own_Below_Threat();
                need_downward_RA = Non_Crossing_Biased_Descend() && Own_Above_Threat();
                if (need_upward_RA && need_downward_RA)
                    /* unreachable: requires Own_Below_Threat and Own_Above_Threat
                       to both be true - that requires Own_Tracked_Alt < Other_Tracked_Alt
                       and Other_Tracked_Alt < Own_Tracked_Alt, which isn't possible */
                    alt_sep = UNRESOLVED;
                else if (need_upward_RA)
                    alt_sep = UPWARD_RA;
                else if (need_downward_RA)
                    alt_sep = DOWNWARD_RA;
                else
                    alt_sep = UNRESOLVED;
            }

            return alt_sep;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {

            System.Console.WriteLine("Mike");
            //Console.WriteLine("arg = {0}", args[0]);


            if (args.Length < 12)
            {
                System.Console.WriteLine("Bad parameters");
                /*fprintf(stdout, "Error: Command line arguments are\n");
                fprintf(stdout, "Cur_Vertical_Sep, High_Confidence, Two_of_Three_Reports_Valid\n");
                fprintf(stdout, "Own_Tracked_Alt, Own_Tracked_Alt_Rate, Other_Tracked_Alt\n");
                fprintf(stdout, "Alt_Layer_Value, Up_Separation, Down_Separation\n");
                fprintf(stdout, "Other_RAC, Other_Capability, Climb_Inhibit\n");
                exit(1);*/
            }
            initialize();
            Cur_Vertical_Sep = Int32.Parse(args[0]); // atoi(argv[1]);
            High_Confidence = Convert.ToBoolean(Int32.Parse(args[1])); // atoi(argv[2]);
            Two_of_Three_Reports_Valid = Convert.ToBoolean(Int32.Parse(args[2])); // atoi(argv[3]);
            Own_Tracked_Alt = Int32.Parse(args[3]); // atoi(argv[4]);
            Own_Tracked_Alt_Rate = Int32.Parse(args[4]); // atoi(argv[5]);
            Other_Tracked_Alt = Int32.Parse(args[5]); // atoi(argv[6]);
            Alt_Layer_Value = Int32.Parse(args[6]); // atoi(argv[7]);
            Up_Separation = Int32.Parse(args[7]); // atoi(argv[8]);
            Down_Separation = Int32.Parse(args[8]); // atoi(argv[9]);
            Other_RAC = Int32.Parse(args[9]); // atoi(argv[10]);
            Other_Capability = Int32.Parse(args[10]); // atoi(argv[11]);
            Climb_Inhibit = Int32.Parse(args[11]); // atoi(argv[12]);

            int val = alt_sep_test();
            System.Console.WriteLine(val);

            //fprintf(stdout, "%d\n", alt_sep_test());
            //exit(0);
        }
    }
}
