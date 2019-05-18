using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace PiersonProject
{
    class item
    {
        static public double lamb=0;
        static public double N = 0;

        public int Xi { get; set; }

        public int Ni { get; set; }

        public double XiNi
        {
            get
            {
                return this.Xi * this.Ni;
            }
        }

        public double Pi
        {
            get
            {
                return Math.Round((Math.Pow(item.lamb, this.Xi) / ExtensionClass.factorial(this.Xi)) * Math.Pow(Math.E, -item.lamb),8);
            }
        }

        public double NPi
        {
            get
            {
                return Math.Round (item.N * this.Pi,8);
            }
        }

        public double value
        {
            get
            {
                return Math.Round (Math.Pow(this.Ni - this.NPi, 2) / this.NPi,8);
            }
        }
    }

    static class ExtensionClass
    {
        static public long factorial(int value)
        {
            long sum = 1;

            for (int i = 1; i <= value; ++i)
            {

                sum *= i;
            }

            return sum;

        }

        static public List<item> generateNumbers()
        {
            List<item> items = new List<item>();

            List<int> numbers = new List<int>();

            Random rand_value = new Random();

            for(int i = 0; i < 500; ++i)
            {
                numbers.Add(rand_value.Next(6));  
            }

            numbers.Sort();

            List<int> numbers_Set = new HashSet<int>(numbers).ToList<int>();

            List<int> frequency = (from i in numbers_Set select numbers.Count((j)=> { return j == i; })).ToList<int>();

            for(int i = 0; i < numbers_Set.Count; ++i)
            {
                items.Add(new item() {Xi=numbers_Set[i],Ni=frequency[i] });
            }

            return items; ;
        }

        static public bool checkSequence(List<item> values)
        {
            bool iscool = false;

            foreach(item i in values)
            {
                if(i.NPi<10 || i.Ni < 5)
                {
                    iscool = true;

                    break;
                }
            }

            return iscool;
        }

        static public bool checkItem(item item_Val)
        {
            bool iscool = false;

            if(item_Val.NPi<10 || item_Val.Ni < 5)
            {
                iscool = true;
            }

            return iscool;
        }

        static public List<item> mergeCells(List<item> values)
        {
            while (checkSequence(values))
            {       
                for(int i = 0; i < values.Count; ++i)
                {
                    if (values.Count == 2)
                    {
                        break;
                    }

                    if (checkItem(values[i]))
                    {
                        if (i == values.Count - 1)
                        {
                            values[i - 1].Ni += values[i].Ni;

                            values.Remove(values[i]);
                        }
                        else
                        {
                            values[i+1].Ni += values[i].Ni;

                            values.Remove(values[i]);
                        }

                        item.lamb = values.Sum(delegate (item val) { return val.XiNi; })/item.N;
                    }
                }

                if (values.Count == 2)
                {
                    break;
                }

            }

            return values;
        }

        static public List<item> generateFromFile
            (string path)
        {
            string[] mass=File.ReadAllLines(path);

                List<int> values = (from j in mass[0].Split(' ') select int.Parse(j)).ToList();
                List<int> frequency = (from j in mass[1].Split(' ') select int.Parse(j)).ToList();

            List<item> items = new List<item>();

            for(int i = 0; i < values.Count; ++i)
            {
                items.Add(new item() { Xi = values[i], Ni = frequency[i] });
            }


            return items ;
        }

    }
    
}