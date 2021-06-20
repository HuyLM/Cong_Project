using System;
using UnityEngine;

namespace AtoLib.UI
{
    public class DOTweenFrame : Frame
    {

        [Header("[Animations]")]
        [SerializeField] protected DOTweenAnimation showAnimation;
        [SerializeField] protected DOTweenAnimation hideAnimation;
        [SerializeField] protected DOTweenAnimation pauseAnimation;
        [SerializeField] protected DOTweenAnimation resumeAnimation;

        protected override void OnInitialize(HUD hud)
        {
            InitializeAnimation();
        }

        protected override void OnShow(Action onCompleted = null, bool instant = false)
        {

            hideAnimation?.ResetState();

            this.gameObject.SetActive(true);

            if (instant || !showAnimation)
            {
                OnShowAnimationCompleted();
                onCompleted?.Invoke();
            }
            else
            {
                showAnimation.Play(() =>
                {
                    OnShowAnimationCompleted();
                    onCompleted?.Invoke();
                }, true);
            }
        }

        protected override void OnHide(Action onCompleted = null, bool instant = false)
        {

            showAnimation?.ResetState();

            if (instant || !hideAnimation)
            {
                this.gameObject.SetActive(false);
                OnHideAnimationCompleted();
                onCompleted?.Invoke();
            }
            else
            {
                hideAnimation.Play(() =>
                {
                    this.gameObject.SetActive(false);
                    OnHideAnimationCompleted();
                    onCompleted?.Invoke();
                },
                true);
            }
        }

        protected override void OnPause(Action onCompleted = null, bool instant = false)
        {

            resumeAnimation?.ResetState();

            if (instant || !pauseAnimation)
            {
                OnPauseAnimationCompleted();
                onCompleted?.Invoke();
            }
            else
            {
                pauseAnimation.Play(() =>
                {
                    OnPauseAnimationCompleted();
                    onCompleted?.Invoke();
                }, true);
            }
        }

        protected override void OnResume(Action onCompleted = null, bool instant = false)
        {

            pauseAnimation?.ResetState();

            if (instant || !resumeAnimation)
            {
                OnResumeAnimationCompleted();
                onCompleted?.Invoke();
            }
            else
            {
                resumeAnimation.Play(() =>
                {
                    OnResumeAnimationCompleted();
                    onCompleted?.Invoke();
                }, true);
            }
        }

        private void InitializeAnimation()
        {
            showAnimation?.Initialize();
            hideAnimation?.Initialize();
            pauseAnimation?.Initialize();
            resumeAnimation?.Initialize();
        }

        public virtual Frame SetAnimShow(bool type)
        {
            return this;
        }
        public virtual Frame SetAnimHide(bool type)
        {
            return this;
        }

        protected virtual void OnShowAnimationCompleted()
        {

        }

        protected virtual void OnHideAnimationCompleted()
        {

        }

        protected virtual void OnPauseAnimationCompleted()
        {

        }

        protected virtual void OnResumeAnimationCompleted()
        {

        }
    }
}