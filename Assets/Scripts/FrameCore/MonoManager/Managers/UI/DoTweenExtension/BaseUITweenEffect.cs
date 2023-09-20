using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public abstract class BaseUITweenEffect
    {
        protected CanvasGroup canvasGroup;

        public BaseUITweenEffect()
        {

        }

        public BaseUITweenEffect(CanvasGroup canvasGroup)
        {
            this.canvasGroup = canvasGroup;
        }

        public abstract void ExecuteEffect();
    }
}


