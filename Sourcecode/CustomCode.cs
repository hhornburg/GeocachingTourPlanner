using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner
{
	/// <summary>
	/// By Standard you can't serialize a KeyValuePair
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	[Serializable]
	public class KeyValuePair<K, V>
	{
		public K Key { get; set; }
		public V Value { get; set; }
		public KeyValuePair()
		{

		}
		public KeyValuePair(K Key, V Value)
		{
			this.Key = Key;
			this.Value = Value;
		}

	}

	#region SortableBindingList
	/// <summary>
	/// Bindinglist is not sortable. This code makes sorting possible
	/// </summary>
	/// <see cref="https://www.codecisions.com/datagridview-sorting-using-custom-bindinglist/"/>
	/// <typeparam name="T"></typeparam>
	public class SortableBindingList<T> : BindingList<T>
	{
		//fields
		private bool SortingInProgress;
		private bool isSorted;
		private ISortComparer<T> sortComparer;
		private ListSortDirection sortDirection;
		private PropertyDescriptor sortProperty;

		/// <summary>
		/// Raised when the list is sorted.
		/// </summary>
		public event EventHandler Sorted;

		//Constructors
		public SortableBindingList()
		{
			SortComparer = new GenericSortComparer<T>();
		}

		public SortableBindingList(IEnumerable<T> contents)
		{
			if(contents != null)
			{
				AddList(contents);
			}
				
			SortComparer = new GenericSortComparer<T>();
		}

		public SortableBindingList(IEnumerable<T> contents, ISortComparer<T> comparer)
		{
			if (contents != null)
				AddList(contents);

			if (comparer == null)
				SortComparer = new GenericSortComparer<T>();
			else
				SortComparer = comparer;
		}

		//Properties
		public ISortComparer<T> SortComparer
		{
			get { return sortComparer; }
			set
			{
				if (value != null)
				{
					sortComparer = value;
				}
				else
				{
					throw new ArgumentNullException("SortComparer", "Value cannot be null.");
				}
			}
		}
		
		protected override bool IsSortedCore
		{
			get { return isSorted; }
		}

		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get { return sortDirection; }
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get { return sortProperty; }
		}


		//Methods
		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
		{
			if (property != null)
			{
				SortingInProgress = true;
				sortDirection = direction;
				sortProperty = property;
				SortComparer.SortProperty = property;
				SortComparer.SortDirection = direction;
				((List<T>)Items).Sort(SortComparer);
				SortingInProgress = false;
				isSorted = true;
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
				OnSortingDone(null, new EventArgs());
			}		
		}

		protected override void RemoveSortCore()
		{
			throw new NotSupportedException();
		}

		protected override void OnListChanged(ListChangedEventArgs e)
		{
			if (!SortingInProgress)
				base.OnListChanged(e);
		}

		protected virtual void OnSortingDone(object sender, EventArgs e)
		{
			if (Sorted != null)
				Sorted(sender, e);
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			if (!SortingInProgress)
				ApplySortCore(SortPropertyCore, SortDirectionCore);
		}

		protected override void SetItem(int index, T item)
		{
			base.SetItem(index, item);
			if (!SortingInProgress)
				ApplySortCore(SortPropertyCore, SortDirectionCore);
		}

		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
			if (!SortingInProgress)
				ApplySortCore(SortPropertyCore, SortDirectionCore);
		}

		protected override void ClearItems()
		{
			base.ClearItems();
		}

		public void AddList(IEnumerable<T> items)
		{
			if (items != null)
				foreach (T item in items)
					Items.Add(item);
		}
	}

	public class GenericSortComparer<T> : ISortComparer<T>
	{
		//Properties
		public PropertyDescriptor SortProperty { get; set; }
		public ListSortDirection SortDirection { get; set; }

		//Constructors
		public GenericSortComparer(){}

		public GenericSortComparer(string sortProperty, ListSortDirection sortDirection) : this(TypeDescriptor.GetProperties(typeof(T)).Find(sortProperty, true), sortDirection) {}

		public GenericSortComparer(PropertyDescriptor sortProperty, ListSortDirection sortDirection)
		{
			SortDirection = sortDirection;
			SortProperty = sortProperty;
		}

		public int Compare(T a, T b)
		{
			if (SortProperty != null)
			{
				IComparable a_comparable = SortProperty.GetValue(a) as IComparable;
				IComparable b_comparable = SortProperty.GetValue(b) as IComparable;
				if (a_comparable == null || b_comparable == null)
					return 0;

				if (SortDirection == ListSortDirection.Ascending)
					return (a_comparable.CompareTo(b_comparable));
				else
					return (b_comparable.CompareTo(a_comparable));
			}
			else
			{
				return 0;
			}
			
		}
	}

	public interface ISortComparer<T> : IComparer<T>
	{
		PropertyDescriptor SortProperty { get; set; }
		ListSortDirection SortDirection { get; set; }
	}
	#endregion
	
	[Serializable()]
	public class SerializableItineroProfile : ISerializable
	{
		public Itinero.Profiles.Profile profile { get; set; }

		/// <summary>
		/// returns true if profile was found
		/// </summary>
		/// <param name="vehicle"></param>
		/// <returns></returns>
		public SerializableItineroProfile(string vehicle, string metric)
		{
			profile = FindProfile(vehicle, metric);
		}

		//Deserialization constructor.
		public SerializableItineroProfile(SerializationInfo info, StreamingContext ctxt)
		{
			//Get the values from info and assign them to the appropriate properties
			string vehicle = (string)info.GetValue("Vehicle", typeof(string));
			string metric = (string)info.GetValue("Metric", typeof(string));

			profile=FindProfile(vehicle, metric);
		}

		//Serialization function.
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("Vehicle", profile.Name);
			info.AddValue("Metric", profile.Metric);
		}

		private Itinero.Profiles.Profile FindProfile(string vehicle, string metric)
		{
			Itinero.Profiles.Vehicle vehicleobject = null;
			switch (vehicle)
			{
				case "Car":
					vehicleobject = Itinero.Osm.Vehicles.Vehicle.Car;
					break;
				default:
					return null;
			}

			switch (metric)
			{
				case "Distance":
					return vehicleobject.Shortest();
				case "Time":
					return vehicleobject.Fastest();
				default:
					return null;
			}

		}
	}
}
