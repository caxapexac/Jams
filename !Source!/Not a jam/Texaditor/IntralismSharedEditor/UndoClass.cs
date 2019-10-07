using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntralismSharedEditor
{
    /// <summary>
    /// отменяет действия
    /// </summary>
    public class UndoClass
    {
        /// <summary>
        /// коструктор
        /// </summary>
        public UndoClass()
        {
            UndoStack = new Stack<Event>();
            RedoStack = new Stack<Event>();  
        }

        /// <summary>
        /// Стек всех действий в текущей сессии
        /// </summary>
        public Stack<Event> UndoStack;//Ctrl + Z

        /// <summary>
        /// Стек всех действий в текущей сессии
        /// </summary>
        public Stack<Event> RedoStack;//Ctrl + Z

        /// <summary>
        /// отменяет последнее действие
        /// </summary>
        public Event Undo()
        {
            if (UndoStack.Count == 0)
            {
                return null;
            }
            else
            {
                Event undoOne = UndoStack.Pop();
                return undoOne;
            }
            
        }

        /// <summary>
        /// отменяет отменённое действие
        /// </summary>
        public Event Redo()
        {
            if (RedoStack.Count == 0)
            {
                return null;
            }
            else
            {
                Event redoOne = RedoStack.Pop();
                return redoOne;
            }
        }

    }
}
