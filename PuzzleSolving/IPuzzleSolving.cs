using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public interface IPuzzleSolving
    {
        void Start();

        void Stop();

        Answer GetAnswer();

        event EventHandler FindBestAnswer;

        event EventHandler FindBetterAnswer;

        event EventHandler SolvingError;
    }
}
