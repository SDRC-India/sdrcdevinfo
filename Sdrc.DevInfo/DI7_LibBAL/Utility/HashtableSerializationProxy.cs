using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;


namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// This class is used as a proxy to enable Hashtable object to be XML serialized and Deserialized.
    /// Each object in Hashtable's collection is stored in Dictionarty Entry, which is serialized and deserialized.
    /// </summary>
    [Serializable()]
    public class HashtableSerializationProxy : ICollection
    {
        [XmlIgnore()]
        public Hashtable _hashTable = new Hashtable();

        [XmlIgnore()]   //innerDictionaryEntry holds the entry for innerHashtableProxy object inside of a DictionaryEntry.
        DictionaryEntry _InnerDictionaryEntry = new DictionaryEntry();

        //[XmlIgnore()]   //InnerHashtableProxy holds the Hashtable (if any) inside of a hashtable.
        //HashtableSerializationProxy _InnerHashTableProxy = new HashtableSerializationProxy();

        private IDictionaryEnumerator _enumerator = null;
        private int _position = -1;

        public HashtableSerializationProxy(Hashtable ht)
        {
            _hashTable = ht;
            _position = -1;
        }
        public HashtableSerializationProxy()
        {
            //Parameterless constructor is required for DeSerialization of class
        }
        // Serialization: XmlSerializer uses this one to get one item at the time
        public DictionaryEntry this[int index]
        {
            get
            {
                if (_enumerator == null)  // lazy initialization
                    _enumerator = _hashTable.GetEnumerator();

                // Accessing an item that is before the current position is something that 
                // shouldn't normally happen because XmlSerializer calls indexer with a constantly 
                // increasing index that starts from zero. 
                // Trying to go backward requires the reset of the enumerator, followed by appropriate 
                // number of increments. (Enumerator acts as a forward-only iterator.)
                if (index < _position)
                {
                    _enumerator.Reset();
                    _position = -1;
                }

                while (_position < index)
                {
                    _enumerator.MoveNext();
                    _position++;
                }
                //Checking if the object contained in Hashtable is also a Hashtable, then_
                // convert same object into HashtableSerializationProxy object.
                //e.g: objectH in Hashtable[Key, ObjectH] is also a Hashtable..,
                // then objectH must converted into HashtableSerializationProxy, i.e. Hahtable[Key, new HashtableSerializationProxy(objectH)]
                try
                {
                    if (_enumerator.Entry.Value.GetType() == typeof(Hashtable))
                    {
                        HashtableSerializationProxy _InnerHashTableProxy = new HashtableSerializationProxy((Hashtable)_enumerator.Entry.Value);
                        _InnerDictionaryEntry = new DictionaryEntry(_enumerator.Entry.Key, _InnerHashTableProxy);

                        return _InnerDictionaryEntry;
                    }
                    else
                    {
                        return _enumerator.Entry;
                    }
                } 
                catch(Exception ex) 
                {
                    return _InnerDictionaryEntry = new DictionaryEntry(_enumerator.Entry.Key, "");   
                }
            }
        }
        // Deserialization: XmlSerializer uses this one to write content back
        public void Add(DictionaryEntry de)
        {
            if (de.Value.GetType() == typeof(HashtableSerializationProxy))
            { _hashTable[de.Key] = ((HashtableSerializationProxy)de.Value)._hashTable; }
            else
            { _hashTable[de.Key] = de.Value; }
        }

        // The rest is a simple redirection to Hashtable's ICollection implementation
        public int Count { get { return _hashTable.Count; } }
        public bool IsSynchronized { get { return _hashTable.IsSynchronized; } }
        public object SyncRoot { get { return _hashTable.SyncRoot; } }
        public void CopyTo(Array array, int index) { _hashTable.CopyTo(array, index); }
        public IEnumerator GetEnumerator() { return _hashTable.GetEnumerator(); }
    }  
}
