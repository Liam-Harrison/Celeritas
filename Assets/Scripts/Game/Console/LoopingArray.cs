using System;
using System.Collections;
using System.Collections.Generic;

namespace Celeritas.Game.Console
{
	public class LoopingArray<T> : IEnumerator<T>, IEnumerable<T> where T: new()
	{
		private int offset;
		private int length;
		private T[] array;
		private int index = 0;

		public T Current => GetValue(index);

		object IEnumerator.Current => Current;

		public int Length { get => length; }

		public LoopingArray(int length)
		{
			array = new T[length];
			offset = 0;
			this.length = 0;
		}

		public T this[int key]
		{
			get
			{
				return GetValue(key);
			}
			set
			{
				SetValue(value, key);
			}
		}

		public T GetValue(int index)
		{
			if (index < 0 || index >= length) throw new ArgumentOutOfRangeException("index", index, "Index out of range of array");

			return array[CalculateIndex(index)];
		}

		public void SetValue(T value, int index)
		{
			if (index < 0 || index >= length) throw new ArgumentOutOfRangeException("index", index, "Index out of range of array");

			array[CalculateIndex(index)] = value;
		}

		public ref T GetRefValue(int index)
		{
			if (index < 0 || index >= length) throw new ArgumentOutOfRangeException("index", index, "Index out of range of array");

			return ref array[CalculateIndex(index)];
		}

		public void Add(T value)
		{
			if (length < array.Length)
			{
				array[length] = value;
				length++;
			}
			else
			{
				offset = CalculateIndex(1);
				int index = CalculateIndex(length - 1);
				array[index] = value;
			}
		}

		public ref T GetNextAvailable()
		{
			if (length < array.Length)
			{
				length++;
				return ref array[length - 1];
			}
			else
			{
				offset = CalculateIndex(1);
				int index = CalculateIndex(length - 1);
				return ref array[index];
			}
		}

		public void Clear()
		{
			Array.Clear(array, 0, array.Length);
			length = 0;
			offset = 0;
		}

		private int CalculateIndex(int index)
		{
			if (index < 0 || index >= length) throw new ArgumentOutOfRangeException("index", index, "Index out of range of array");

			int actual = offset + index;
			if (actual < length) return actual;
			else return actual - array.Length;
		}

		public T First()
		{
			if (0 == length) return default;
			else return GetValue(CalculateIndex(0));
		}

		public ref T GetFirstRef()
		{
			if (0 == length) throw new ArgumentOutOfRangeException("index", 0, "Index out of range of array");
			else return ref array[0];
		}

		public T Last()
		{
			if (0 == length) return default;
			else return GetValue(Length - 1);
		}

		public ref T GetLastRef()
		{
			if (0 == length) throw new ArgumentOutOfRangeException("index", 0, "Index out of range of array");
			else return ref array[CalculateIndex(Length - 1)];
		}

		public bool MoveNext()
		{
			index++;

			return index < length;
		}

		public void Reset()
		{
			index = 0;
		}

		public void Dispose()
		{

		}

		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}
	}
}
