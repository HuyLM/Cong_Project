﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace AtoLib {
    public abstract class CollectionDisplayer<T> : MonoBehaviour {
        protected int Capacity { get; private set; }
        protected T[] Items { get; private set; }
        public abstract int DisplayerCount { get; }

        public T GetItem(int index) {
            if (Items == null) {
                return default;
            }
            if (index < 0 || index >= Items.Length) {
                return default;
            }
            return Items[index];
        }

        public IEnumerable<T> GetAllItem() {
            if (Items != null) {
                foreach (T item in Items) {
                    yield return item;
                }
            }
        }

        public abstract void Show();

        public CollectionDisplayer<T> SetItems(IEnumerable<T> items) {
            Items = items.ToArray();
            return this;
        }

        public CollectionDisplayer<T> SetItems(params T[] items) {
            Items = items;
            return this;
        }

        public CollectionDisplayer<T> SetItems(List<T> items) {
            Items = items.ToArray();
            return this;
        }

        public CollectionDisplayer<T> SetCapacity(int capacity) {
            Capacity = capacity;
            return this;
        }
    }
}


