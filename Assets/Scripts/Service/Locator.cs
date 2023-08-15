using System;
using DefaultNamespace;
using DefaultNamespace.Model.Minefield;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using View;

namespace Service
{
    public class Locator : MonoBehaviour
    {
        public IMineField MineField => _mineField;
        public IFieldDataProvider FieldDataProvider => _fieldDataProvider;
        public FieldController FieldController => _fieldController;
        public CellImagesConfig CellImagesConfig => _cellImagesConfig;
        public CellView CellPrefab => cellPrefab;
        public MineFieldViewData MineFieldViewData => _mineFieldViewData;
        public IGameStateModel GameStateModel => _gameStateModel;
        public IGameSettings GameSettings => gameSettings;
        
        [SerializeField]
        private SetCountFieldDataProvider.Settings _fieldDataProviderSettings;
        [SerializeField]
        private MinefieldGenerator.Settings _mineFieldGeneratorSettings;
        [SerializeField]
        private FieldController _fieldController;
        [SerializeField]
        private CellImagesConfig _cellImagesConfig;
        [SerializeField]
        private CellView cellPrefab;
        [SerializeField] 
        private GameSettings gameSettings;
        
        private IFieldDataProvider _fieldDataProvider;
        private IMineField _mineField;
        private MineFieldViewData _mineFieldViewData;
        private GameStateModel _gameStateModel;

        private void ResolveDependencies()
        {
            _fieldDataProvider = new SetCountFieldDataProvider(_fieldDataProviderSettings);
            _mineField = new Minefield(_mineFieldGeneratorSettings.FieldSizeX, _mineFieldGeneratorSettings.FieldSizeY);
            _mineFieldViewData = new MineFieldViewData(_mineField);
            _gameStateModel = new GameStateModel();
        }

        #region SINGLETON

        public static Locator Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;

            ResolveDependencies();
        }

        #endregion
    }
}