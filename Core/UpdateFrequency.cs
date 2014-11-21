using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.RichardLord.Ash.Core
{
    public enum UpdateFrequency
    {
        EveryFrame,
        EveryOtherFrame,
        Every10Frames,
        IfComponentCountChanges,
        Never
    }
}
