using System.Collections.Generic;

namespace System.Util
{
    public class HashMap<K, V> : Object
    {

        public class Entry
        {
            public K key;
            public V value;

            public Entry(K key, V value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public List<Entry> entries = new List<Entry>();

        public HashMap()
        {
            entries = new List<Entry>();
        }

        public void Put(K key, V value)
        {
            entries.Add(new Entry(key, value));
        }

        public void Replace(K key, V value)
        {

            foreach (Entry e in entries)
            {
                if (e.key.Equals(key))
                {
                    e.value = value;
                    return;
                }
            }

            Entry entry = new Entry(key, value);

            entries.Add(entry);
        }

        public V Get(K key)
        {
            foreach (Entry e in entries)
            {
                if (e.key.Equals(key))
                    return e.value;
            }

            return default(V);
        }

        public K GetKey(V value)
        {
            foreach (Entry e in entries)
            {
                if (e.value.Equals(value))
                    return e.key;
            }
            return default(K);
        }

        public List<K> KeySet()
        {
            List<K> keySet = new List<K>();

            foreach (Entry e in entries)
            {
                keySet.Add(e.key);
            }

            return keySet;
        }

        public List<V> ValueSet()
        {
            List<V> valueSet = new List<V>();

            foreach (Entry e in entries)
            {
                valueSet.Add(e.value);
            }

            return valueSet;
        }

        public List<Entry> EntrySet()
        {
            return this.entries;
        }

        public int Size()
        {
            return this.entries.Count;
        }

        public bool IsEmpty()
        {
            if (this.entries.Count == 0 || this.entries.Equals(null))
                return true;

            return false;
        }

        public void Remove(K key)
        {
            foreach (Entry e in entries)
            {
                if (e.key.Equals(key))
                    entries.Remove(e);
            }
        }
    }
}

