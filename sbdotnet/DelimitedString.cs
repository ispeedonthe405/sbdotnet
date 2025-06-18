using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sbdotnet
{
	public class DelimitedString : INotifyPropertyChanged
	{
		///////////////////////////////////////////////////////////
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetField<TField>(ref TField field, TField value, string propertyName)
		{
			if (EqualityComparer<TField>.Default.Equals(field, value))
			{
				return;
			}

			field = value;
			OnPropertyChanged(propertyName);
		}

		#endregion INotifyPropertyChanged
		///////////////////////////////////////////////////////////


		/////////////////////////////////////////////////////////
		#region Fields

		private string _OldDelimiter = string.Empty;
		private string _Delimiter = ",";
		private string _Value = string.Empty;
		private ObservableCollection<string> _Collection = [];

		#endregion Fields
		/////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////
		#region Properties

		public string OldDelimiter
		{
			get => _OldDelimiter;
			private set => _OldDelimiter = value;
		}

		public string Delimiter
		{
			get => _Delimiter;
			set
			{
				_OldDelimiter = _Delimiter;
				SetField(ref _Delimiter, value, nameof(Delimiter));
			}
		}

		public string Value
		{
			get => _Value;
			private set => SetField(ref _Value, value, nameof(Value));
		}

		public ObservableCollection<string> Collection
		{
			get => _Collection;
			private set => SetField(ref _Collection, value, nameof(Collection));
		}


		#endregion Properties
		/////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////
		#region Interface

		public DelimitedString()
		{
			PropertyChanged += DelimitedString_PropertyChanged;
			Collection.CollectionChanged += Collection_CollectionChanged;
		}

		public DelimitedString(string delimiter = ",", IEnumerable<string>? collection = null)
		{
			Delimiter = delimiter;

			// Note: Init order is deliberate here. CollectionChanged is hooked after this
			// to prevent a flood of events during AddRange. That's why the explicit call
			// to RebuildFromCollection is there.
			if (collection is not null)
			{
				Collection.AddRange(collection);
			}
			RebuildFromCollection();

			PropertyChanged += DelimitedString_PropertyChanged;
			Collection.CollectionChanged += Collection_CollectionChanged;
		}

		public void Add(string value)
		{
			Collection.Add(value);
		}

		public void Remove(string value)
		{
			Collection.Remove(value);
		}

		public bool Contains(string value)
		{
			return Collection.Contains(value);
		}

		public bool IsNull()
		{
			return _Value.IsNull();
		}

		public override string ToString()
		{
			return _Value;
		}

		public override bool Equals(object? obj)
		{
			return _Value.Equals(obj);
		}

		public override int GetHashCode()
		{
			return _Value.GetHashCode();
		}

		#endregion Interface
		/////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////
		#region Internal

		private void DelimitedString_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName is null) return;

			if (e.PropertyName.Equals(nameof(Delimiter)))
			{
				ChangeDelimiter();
			}
		}

		private void Collection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RebuildFromCollection();
		}

		private void RebuildFromCollection()
		{
			string newValue = string.Empty;
			foreach (var item in Collection)
			{
				if (item == Collection.Last())
				{
					newValue += item;
				}
				else
				{
					newValue += $"{item}{Delimiter}";
				}
			}
			Value = newValue;
		}

		private void ChangeDelimiter()
		{
			Value = _Value.Replace(_OldDelimiter, _Delimiter);
		}

		#endregion Internal
		/////////////////////////////////////////////////////////
	}
}
