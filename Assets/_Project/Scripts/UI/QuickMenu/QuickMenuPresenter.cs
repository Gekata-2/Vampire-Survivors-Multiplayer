using System;
using Zenject;

namespace _Project.Scripts.UI.QuickMenu
{
    public class QuickMenuPresenter : IInitializable, IDisposable
    {
        private readonly QuickMenu _view;
        private readonly QuickMenuModel _model;

        public QuickMenuPresenter(QuickMenu view, QuickMenuModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _view.QuitClicked += OnQuitClicked;
        }

        private void OnQuitClicked()
            => _model.QuitGame();

        public void Dispose()
        {
            _view.QuitClicked -= OnQuitClicked;
        }
    }
}