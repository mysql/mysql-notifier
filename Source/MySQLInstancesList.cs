//
// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

namespace MySql.Notifier
{
  using System.Collections;
  using System.Collections.Generic;
  using MySql.Notifier.Properties;

  /// <summary>
  /// List of <see cref="MySQLInstance"/> objects used to monitor MySQL Server instances.
  /// </summary>
  public class MySQLInstancesList : IList<MySQLInstance>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLInstancesList"/> class.
    /// </summary>
    public MySQLInstancesList()
    {
      if (InstancesList == null)
      {
        InstancesList = new List<MySQLInstance>();
        return;
      }

      //// TODO: Loop through instances and add them to Notifier's context menu and monitor.
      foreach (var instance in InstancesList)
      {
      }
    }

    #region Properties

    /// <summary>
    /// Gets or sets a list of <see cref="MySQLInstance"/> objects representing instances being monitored.
    /// </summary>
    private List<MySQLInstance> InstancesList
    {
      get
      {
        return Settings.Default.MySQLInstancesList;
      }

      set
      {
        Settings.Default.MySQLInstancesList = value;
      }
    }

    #endregion Properties

    #region IList implementation

    /// <summary>
    /// Gets the number of elements actually contained in the list.
    /// </summary>
    public int Count
    {
      get
      {
        return InstancesList.Count;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the collection is read-only
    /// </summary>
    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>A <see cref="MySQLInstance"/> object at the given index position.</returns>
    public MySQLInstance this[int index]
    {
      get
      {
        return InstancesList[index];
      }

      set
      {
        InstancesList[index] = value;
      }
    }

    /// <summary>
    /// Adds a <see cref="MySQLInstance"/> object to the end of the list.
    /// </summary>
    /// <param name="item">A <see cref="MySQLInstance"/> object to add.</param>
    public void Add(MySQLInstance item)
    {
      InstancesList.Add(item);

      //// TODO: Monitor the connection.

      //// TODO: Update the Notifier UI.

      Settings.Default.Save();
    }

    /// <summary>
    /// Removes all elements from the list.
    /// </summary>
    public void Clear()
    {
      InstancesList.Clear();
    }

    /// <summary>
    /// Determines whether an element is in the list.
    /// </summary>
    /// <param name="item">A <see cref="MySQLInstance"/> object.</param>
    /// <returns>true if item is found in the list; otherwise, false.</returns>
    public bool Contains(MySQLInstance item)
    {
      return InstancesList.Contains(item);
    }

    /// <summary>
    /// Copies the entire list to a compatible one-dimensional array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from list. The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(MySQLInstance[] array, int arrayIndex)
    {
      InstancesList.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the list.
    /// </summary>
    /// <returns>An <see cref="Enumerator"/> for the list.</returns>
    public IEnumerator<MySQLInstance> GetEnumerator()
    {
      return InstancesList.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return InstancesList.GetEnumerator();
    }

    /// <summary>
    /// Determines the index of a specific item in the list.
    /// </summary>
    /// <param name="item">The <see cref="MySQLInstance"/> object to locate in the list.</param>
    /// <returns>The index of item if found in the list; otherwise, –1.</returns>
    public int IndexOf(MySQLInstance item)
    {
      return InstancesList.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item to the list at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <seealso cref="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="MySQLInstance"/> object to insert to the list.</param>
    public void Insert(int index, MySQLInstance item)
    {
      InstancesList.Insert(index, item);
    }

    /// <summary>
    /// Removes the first occurrence of a specific <see cref="MySQLInstance"/> object from the list.
    /// </summary>
    /// <param name="item">The <see cref="MySQLInstance"/> object to remove from the list.</param>
    /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the list.</returns>
    public bool Remove(MySQLInstance item)
    {
      int index = InstancesList.IndexOf(item);
      bool success = index >= 0;
      if (success)
      {
        try
        {
          RemoveAt(index);
        }
        catch
        {
          success = false;
        }
      }

      return success;
    }

    /// <summary>
    /// Removes the first occurrence of a specific <see cref="MySQLInstance"/> object with the given id from the list.
    /// </summary>
    /// <param name="connectionId">Id if the connection linked to the instance to remove.</param>
    /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the list.</returns>
    public bool Remove(string connectionId)
    {
      int index = InstancesList.FindIndex(ins => ins.ConnectionId == connectionId);
      bool success = index >= 0;
      if (success)
      {
        try
        {
          RemoveAt(index);
        }
        catch
        {
          success = false;
        }
      }

      return success;
    }

    /// <summary>
    /// Removes the element at the specified index of the list.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    public void RemoveAt(int index)
    {
      InstancesList.RemoveAt(index);

      //// TODO: Stop monitoring the connection.

      //// TODO: Update the Notifier UI.

      Settings.Default.Save();
    }

    #endregion IList implementation
  }
}