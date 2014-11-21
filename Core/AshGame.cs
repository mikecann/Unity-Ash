using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Net.RichardLord.Ash.Core
{
    public class AshGame : MonoBehaviour
    {
        private IGame _engine;
        public IGame Engine
        {
            get
            {
                if (_engine == null) _engine = new Game<ComponentMatchingFamily>();
                return _engine;
            }
            set { _engine = value; }
        }      
  
        void Update()
        {
            Engine.Update(Time.deltaTime);
        }
    }
}
