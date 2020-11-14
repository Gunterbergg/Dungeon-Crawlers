using UnityEngine;
using System;

namespace DungeonCrawlers.UI
{ 
    public class PlayerHealthBar : UserView, IOutputHandler<int>
    {
        public Animator firstHeart;
        public Animator secondtHeart;
        public Animator thirdHeart;

        public int CurrentOutput { get; private set; }

        public void Clear() => Output(0);

        public void Output(int output, Action callback = null)
        {
            CurrentOutput = Mathf.Clamp(output, 0, 3);
            switch (CurrentOutput) {
                case 0: Disable(firstHeart, secondtHeart, thirdHeart); break;
                case 1: Enable(firstHeart); Disable(secondtHeart, thirdHeart); break;
                case 2: Enable(firstHeart, secondtHeart); Disable(thirdHeart); break;
                case 3: Enable(firstHeart, secondtHeart, thirdHeart); break;
            }
            callback?.Invoke();
        }

        private void Enable(params Animator[] animators)
        {
            foreach (Animator anim in animators)
                anim.SetBool("active", true);
        }

        private void Disable(params Animator[] animators) 
        {
            foreach (Animator anim in animators)
                anim.SetBool("active", false);
        }

        public override void Activate() => GetComponent<CanvasGroup>().alpha = 1f;

        public override void DeActivate() => GetComponent<CanvasGroup>().alpha = 0f;

        public void OutputDefault() => Clear();
    }
}