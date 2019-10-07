﻿using System;
using UnityEngine;


namespace Client.Scripts.Algorithms.Legacy
{
    /* Leopotam:
     * я делаю как-то так
     * гружу по имени и типу, висящему на префабе
     * тип делаю максимально тонким и только для управления свойствами ГО
     * в самом простом случае это Transform
     * в ecs делаю компонент с полем T Visual;
     * где T - этот компонент с префаба
     * пулы держу либо локально либо делаю синглтоны
     * и тогда их можно использовать внутри IEcsAutoResetComponent
     *
     * по сути это пул из LeopotamGroup, просто почищен
     * старый - более универсальный, с обработкой ошибок,
     * статичными методами генерации инстансов и тп.
     * тут все нужно делать руками и внимательно следить за тем что грузишь
     * потому что эксепшнов в конструкторе быть не должно
     */
    public sealed class PrefabPool<T> : IDisposable where T : Component
    {
        T[] _items = new T[8];
        int _itemsCount;

        T _prefab;

        public PrefabPool(string prefabPath)
        {
            _prefab = Resources.Load<T>(prefabPath);
        }

        public T Get()
        {
            bool isNew;
            return Get(out isNew);
        }

        public T Get(out bool isNew)
        {
            if (_itemsCount > 0)
            {
                isNew = false;
                return _items[--_itemsCount];
            }
            isNew = true;
            return UnityEngine.Object.Instantiate(_prefab).GetComponent<T>();
        }

        public void Recycle(T item)
        {
            if (_items.Length == _itemsCount)
            {
                Array.Resize(ref _items, _itemsCount << 1);
            }
            _items[_itemsCount++] = item;
        }

        public void Dispose()
        {
            _prefab = null;
            for (var i = 0; i < _itemsCount; i++)
            {
                if (_items[i])
                {
                    UnityEngine.Object.Destroy(_items[i].gameObject);
                }
                _items[i] = null;
            }
            _itemsCount = 0;
        }
    }
}