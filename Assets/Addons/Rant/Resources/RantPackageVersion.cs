﻿#region License

// https://github.com/TheBerkin/Rant
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Globalization;

using Rant.Core.Utilities;

#pragma warning disable 1591 // I am *not* writing XML docs for operator overloads with blatantly obvious usages.

namespace Rant.Resources
{
    /// <summary>
    /// Represents a version number for a Rant package.
    /// </summary>
    public sealed class RantPackageVersion
    {
        private int _major;
        private int _minor;
        private int _revision;

        /// <summary>
        /// Initializes a new RantPackageVersion instance with the specified values.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <param name="revision">The revision number.</param>
        public RantPackageVersion(int major, int minor, int revision)
        {
            if (major < 0) throw new ArgumentException("Major version must be non-negative.");
            if (minor < 0) throw new ArgumentException("Minor version must be non-negative.");
            if (revision < 0) throw new ArgumentException("Revision number must be non-negative.");
            _major = major;
            _minor = minor;
            _revision = revision;
        }

        /// <summary>
        /// Initializes a new RantPackageVersion instance with all values set to zero.
        /// </summary>
        public RantPackageVersion()
        {
        }

        /// <summary>
        /// The major version.
        /// </summary>
        public int Major
        {
            get { return _major; }
            set { _major = value < 0 ? 0 : value; }
        }

        /// <summary>
        /// The minor version.
        /// </summary>
        public int Minor
        {
            get { return _minor; }
            set { _minor = value < 0 ? 0 : value; }
        }

        /// <summary>
        /// The revision number.
        /// </summary>
        public int Revision
        {
            get { return _revision; }
            set { _revision = value < 0 ? 0 : value; }
        }

        public static bool operator <(RantPackageVersion a, RantPackageVersion b)
        {
            if (ReferenceEquals(a, null)) throw new ArgumentNullException();
            if (ReferenceEquals(b, null)) throw new ArgumentNullException();
            return a._major != b._major
                ? a._major < b._major
                : (a._minor == b._minor ? a._revision < b._revision : a._minor < b._minor);
        }

        public static bool operator >(RantPackageVersion a, RantPackageVersion b)
        {
            if (ReferenceEquals(a, null)) throw new ArgumentNullException();
            if (ReferenceEquals(b, null)) throw new ArgumentNullException();
            return a._major != b._major
                ? a._major > b._major
                : (a._minor == b._minor ? a._revision > b._revision : a._minor > b._minor);
        }

        public static bool operator <=(RantPackageVersion a, RantPackageVersion b)
        {
            if (ReferenceEquals(a, null)) throw new ArgumentNullException();
            if (ReferenceEquals(b, null)) throw new ArgumentNullException();
            return a._major != b._major
                ? a._major <= b._major
                : (a._minor == b._minor ? a._revision <= b._revision : a._minor <= b._minor);
        }

        public static bool operator >=(RantPackageVersion a, RantPackageVersion b)
        {
            if (ReferenceEquals(a, null)) throw new ArgumentNullException();
            if (ReferenceEquals(b, null)) throw new ArgumentNullException();
            return a._major != b._major
                ? a._major >= b._major
                : (a._minor == b._minor ? a._revision >= b._revision : a._minor >= b._minor);
        }

        public static bool operator ==(RantPackageVersion a, RantPackageVersion b)
        {
            if (ReferenceEquals(a, null)) return ReferenceEquals(b, null);
            if (ReferenceEquals(b, null)) return false;
            return a._major == b._major && a._minor == b._minor && a._revision == b._revision;
        }

        public static bool operator !=(RantPackageVersion a, RantPackageVersion b)
        {
            if (ReferenceEquals(a, null)) return !ReferenceEquals(b, null);
            if (ReferenceEquals(b, null)) return true;
            return a._major != b._major || a._minor != b._minor || a._revision != b._revision;
        }

        /// <summary>
        /// Attempts to parse a version string and returns the equivalent RantPackageVersion.
        /// </summary>
        /// <param name="version">The version string to parse.</param>
        /// <returns></returns>
        public static RantPackageVersion Parse(string version)
        {
            const NumberStyles styles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
            if (Util.IsNullOrWhiteSpace(version)) throw new ArgumentException();
            var parts = version.Split('.');
            if (parts.Length > 3) throw new FormatException("Version cannot be composed of more than 3 parts.");
            var v = new RantPackageVersion();
            if (!int.TryParse(parts[0], styles, CultureInfo.InvariantCulture, out v._major) || v._major < 0)
                throw new FormatException("Major version must be a valid, non-negative integer.");
            if (parts.Length < 2) return v;
            if (!int.TryParse(parts[1], styles, CultureInfo.InvariantCulture, out v._minor) || v._minor < 0)
                throw new FormatException("Minor version must be a valid, non-negative integer.");
            if (parts.Length < 3) return v;
            if (!int.TryParse(parts[2], styles, CultureInfo.InvariantCulture, out v._revision) || v._revision < 0)
                throw new FormatException("Revision number must be a valid, non-negative integer.");
            return v;
        }

        /// <summary>
        /// Attempts to parse a version string and outputs the equivalent RantPackageVersion.
        /// </summary>
        /// <param name="version">The version string to parse.</param>
        /// <param name="result">The parsing result, if successful.</param>
        /// <returns></returns>
        public static bool TryParse(string version, out RantPackageVersion result)
        {
            const NumberStyles styles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
            result = null;
            if (Util.IsNullOrWhiteSpace(version)) return false;
            var parts = version.Split('.');
            if (parts.Length > 3) return false;
            var v = new RantPackageVersion();
            if (!int.TryParse(parts[0], styles, CultureInfo.InvariantCulture, out v._major) || v._major < 0)
                return false;
            if (parts.Length < 2) return true;
            if (!int.TryParse(parts[1], styles, CultureInfo.InvariantCulture, out v._minor) || v._minor < 0)
                return false;
            if (parts.Length < 3) return true;
            return int.TryParse(parts[2], styles, CultureInfo.InvariantCulture, out v._revision) && v._revision >= 0;
        }

        /// <summary>
        /// Returns a string representation of the current version.
        /// </summary>
        /// <returns></returns>
		public override string ToString() { return _major+"."+_minor+"."+_revision; }
	
        /// <summary>
        /// Determines whether the current version is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var b = obj as RantPackageVersion;
            if (ReferenceEquals(b, null)) return false;
            return this == b;
        }

		public override int GetHashCode ()
		{
			return unchecked((_major + 12345) * (_minor + 47) * _revision * 31);
		}

	}
}