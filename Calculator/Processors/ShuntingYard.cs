using System.Data;

namespace Calculator.Processors;

public class ShuntingYard
{
    private readonly Queue<string> _queue = new();
    private readonly Stack<string> _stack = new();

    public Queue<string> Convert(ref List<string> tokens)
    {
        if (_stack.Count != 0)
            _stack.Clear();
        
        foreach (string element in tokens)
        {
            if (element == "(")
            {
                _stack.Push(element);
            }

            else if (element == ")")
            {
                while (_stack.Count != 0 && _stack.Peek() != "(")
                {
                    _queue.Enqueue(_stack.Peek());
                    _stack.Pop();
                }

                if (_stack.Peek() == "(")
                {
                    _stack.Pop();
                } 
            }
            else if (element.IsNumber())
            {
                _queue.Enqueue(element);
            }

            else if (element.IsFunc())
            {
                _stack.Push(element);
            }
            else if (element.IsOperator())
            {
                while (_stack.Count != 0 && Helpers.ComparePriority(_stack.Pop(), element))
                {
                    _queue.Enqueue(_stack.Peek());
                    _stack.Pop();
                }
                _stack.Push(element);
            }
        }
        while (_stack.Count != 0)
        {
            if (_stack.Peek() != "(")
            {
                _queue.Enqueue(_stack.Peek());
            }
            _stack.Pop();
        }
        
        return _queue;
    }
    
}