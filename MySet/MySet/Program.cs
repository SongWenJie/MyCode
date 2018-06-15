using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySet
{
    class Program
    {
        static void Main(string[] args)
        {
            object[] values = { "a", "b", "c", "d", "e" };
            MySet mySet = new MySet(values);
            foreach (var item in mySet)
            {
                Console.WriteLine(item);
            }


        }
    }

    public class MySet : IEnumerable
    {
        internal object[] values;

        public MySet(object[] values)
        {
            this.values = values;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < values.Length; i++)
            {
                yield return values[i];
            }
        }
    }

    public class MySetIterator : IEnumerator
    {
        MySet set;
        /// <summary>
        /// 保存迭代到的位置
        /// </summary>
        int position;
        internal MySetIterator(MySet set)
        {
            this.set = set;
            position = -1;
        }

        public object Current
        {
            get
            {
                if(position==-1||position==set.values.Length)
                {
                    throw new InvalidOperationException();
                }
                int index = position;
                return set.values[index];
            }
        }

        public bool MoveNext()
        {
            if(position!=set.values.Length)
            {
                position++;
            }
            return position < set.values.Length;
        }

        public void Reset()
        {
            position = -1;
        }
    }

}