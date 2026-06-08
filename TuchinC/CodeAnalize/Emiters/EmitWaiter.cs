using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.CodeAnalize.Emiters
{
    public abstract class EmitWaiter<TValue>() 
        where TValue : struct
    {
        protected readonly List<TValue> EmitedValues = [];
        protected Stack<(int, int)> Waits = [];

        public TValue Index(int index)
        {
            if(index > EmitedValues.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Индекс вышел за границы массива. " +
                    "Похоже была произведенна не правленая отправка значений");

           return EmitedValues[index];
        }
        public TValue[] IndexRange(int index, int length) => [.. EmitedValues.GetRange(index, length)];

        protected int EmitWait()
        {
            EmitedValues.Add(default);

            (int, int) wait = (EmitedValues.Count-1, 1);
            Waits.Push(wait);

            return EmitedValues.Count - 1;
        }
       
        protected (int, int) EmitWaitRange(int length)
        {
            for (int i = 0; i < length; i++)
                EmitedValues.Add(default);

            (int, int) wait = (EmitedValues.Count - 1, length);
            Waits.Push(wait);

            for (int i = 0; i < length; i++)
                EmitedValues.Add(default);

            return wait;
        }

        protected void EmitQuit(TValue value)
        {
            if (Waits.Count == 0)
                return;

            var wait = Waits.Pop();
            EmitedValues[wait.Item1] = value;
        }
        protected void EmitQuitRange(TValue[] values)
        {
            if (Waits.Count == 0)
                return;

            var wait = Waits.Pop();

            if (values.Length > wait.Item2 || values.Length < wait.Item2)
                throw new ArgumentOutOfRangeException(nameof(values), "Длинна куcка байтов вышла за границы требуемой длинны отрезка");

            if (wait.Item1 > EmitedValues.Count - 1 || wait.Item2 > EmitedValues.Count)
                throw new ArgumentOutOfRangeException(nameof(values), "Индекс или длинна вышли за гранницы массива байтов");


            for (int i = wait.Item1; i < wait.Item2; i++)
                EmitedValues[i] = values[i % wait.Item1];
        }
    }
}
