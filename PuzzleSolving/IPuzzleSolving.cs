using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    interface IPuzzleSolving
    {
        void Start();

        void Stop();

        String GetAnswerString();

        int GetAnswerCost();

        event EventHandler FindBestAnswer;

        event EventHandler FindBetterAnswer;

        event EventHandler SolvingError;
    }
}
