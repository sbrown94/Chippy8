namespace Chippy8.Core
{
    public class Stack : IStack
    {
        public int stackPtr;
        public byte[] stack;

        public Stack()
        {
            stackPtr = 0;
            stack = new byte[32]; // 16 16 bit values
        }

        public byte Pop()
        {
            stackPtr--;
            return stack[stackPtr+1];
        }

        public void Push(int value)
        {
            stackPtr++;
            stack[stackPtr] = value;
        }
    }
}
