/*
    Copyright (C) 2014-2019 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace dnSpy.Contracts.Debugger.DotNet.CorDebug {
	/// <summary>
	/// Debugging options that will start and debug an application when passed to <see cref="DbgManager.Start(DebugProgramOptions)"/>.
	/// This is used to debug .NET Framework assemblies.
	/// </summary>
	public sealed class DotNetFrameworkStartDebuggingOptions : CorDebugStartDebuggingOptions {
		/// <summary>
		/// Version of an already installed CLR (eg. "v2.0.50727", or "v4.0.30319") or null to auto detect it
		/// </summary>
		public string? DebuggeeVersion { get; set; }

		/// <summary>
		/// Clones this instance
		/// </summary>
		/// <returns></returns>
		public override DebugProgramOptions Clone() => CopyTo(new DotNetFrameworkStartDebuggingOptions());

		/// <summary>
		/// Copies this instance to <paramref name="other"/> and returns it
		/// </summary>
		/// <param name="other">Destination</param>
		/// <returns></returns>
		public DotNetFrameworkStartDebuggingOptions CopyTo(DotNetFrameworkStartDebuggingOptions other) {
			if (other is null)
				throw new ArgumentNullException(nameof(other));
			base.CopyTo(other);
			other.DebuggeeVersion = DebuggeeVersion;
			return other;
		}
	}
}
