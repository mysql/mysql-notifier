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

using System;
using System.Security.Cryptography;
using System.Text;

namespace MySql.Notifier
{
  /// <summary>
  /// Store and recover account login information to access and interact with remote machines.
  /// </summary>
  public static class MySqlSecurity
  {
    /// <summary>
    /// Special Character for password separator
    /// </summary>
    private const string PASSWORD_ENTROPY = "☻♥";

    /// <summary>
    /// Security
    /// </summary>
    private const DataProtectionScope CURRENT_USER_SCOPE = DataProtectionScope.CurrentUser;

    /// <summary>
    /// Decrypts an 64 based string.
    /// </summary>
    /// <param name="encryptedString">Password to be decrypted.</param>
    /// <returns>Encrypted password is returned as a string.</returns>
    public static string DecryptPassword(string encryptedString)
    {
      if (encryptedString == null)
      {
        throw new ArgumentNullException("Encrypted password should not be null.");
      }

      var encryptedData = Convert.FromBase64String(encryptedString);
      var optionalEntropy = Encoding.Unicode.GetBytes(PASSWORD_ENTROPY);

      // Decrypting string
      byte[] decryptedPassword = ProtectedData.Unprotect(encryptedData, optionalEntropy, CURRENT_USER_SCOPE);

      return Encoding.Unicode.GetString(decryptedPassword);
    }

    /// <summary>
    /// Encrypts a system string.
    /// </summary>
    /// <param name="unencryptedString">Password to be encrypted.</param>
    /// <returns>Encrypted Password is returned as a 64 base string.</returns>
    public static string EncryptPassword(string unencryptedString)
    {
      if (unencryptedString == null)
      {
        throw new ArgumentNullException("Unencrypted String cannot be null");
      }

      var unencryptedData = Encoding.Unicode.GetBytes(unencryptedString);
      var optionalEntropy = Encoding.Unicode.GetBytes(PASSWORD_ENTROPY);

      // Encrypting string
      byte[] encryptedPassword = ProtectedData.Protect(unencryptedData, optionalEntropy, CURRENT_USER_SCOPE);

      return Convert.ToBase64String(encryptedPassword);
    }
  }
}