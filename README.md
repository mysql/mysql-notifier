MySQL Notifier 1.1
=========
MySQL Notifier is a small utility that can keep watch over your Windows and MySQL services, notifying you of changes in their operation. Notifier is intended to run when Windows starts up and sits in the tray notification area watching the MySQL and Windows services you have configured.
Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.

## Installation

* Prerequisites:
	* Visual Studio 2010 or greater.
	* .NET Framework 4.0 (Client or Full Profile).
	* WiX Toolset, for building the installer MSI.
* Open MySQLNotifier.sln in Visual Studio.

## Features

* Monitor Windows Services
	* MySQL Notifier watches your selected services and MySQL database connections for changes. When a monitored service changes state, you are notified using a notification balloon. You can then use the context menu for that service to manage the state of that service or possibly launch directly into MySQL Workbench. Service monitoring is limited to Windows computers, however monitoring of database connections is fully cross platform.
* Monitor MySQL Connections
	* When you have a farm of MySQL instances that you want to keep an eye on, Notifier can help. Simply create the connection using Notifier or in Workbench and you can easily monitor it with Notifier. Notifier will keep an eye on the server using the connection given. If the server stops responding Notifier will gently nudge you taht something could be wrong. You can then go to Workbench or MySQL Enterprise Monitor for more detailed analysis.
* Integration With MySQL Workbench
	* MySQL Notifer is fully connected to MySQL Workbench. In fact, they use the same connection list. So create a connection in Workbench and see if appear automatically in MySQL Notifier. If you are interested in a server enough to use it in Workbench then you likely want to keep an eye on it in Notifier. Also, directly from the server context menu in Notifier you can launch directly to either the Workbench Administration page or to the SQL Editor.
* Integration with MySQL Installer
	* There's nothing worse than letting your software get out of date. That's why Notifier will use the new MySQL Installer to monitor your installed MySQL software and let you know when there are software updates available. If so, it will give you a gentle reminder along with a context menu to launch directly to MySQL Installer. Simple.

## Documentation

For further information about MySQL or additional documentation, see:
* http://www.mysql.com
* http://dev.mysql.com/doc/refman/5.7/en/windows-notifier.html

## License

License information can be found in the Installer/COPYING file.

__MySQL FOSS License Exception__
We want free and open source software applications under certain licenses to be able to use specified GPL-licensed MySQL client libraries despite the fact that not all such FOSS licenses are compatible with version 2 of the GNU General Public License.
Therefore there are special exceptions to the terms and conditions of the GPLv2 as applied to these client libraries, which are identified and described in more detail in the FOSS License Exception at http://www.mysql.com/about/legal/licensing/foss-exception.html.

This distribution may include materials developed by third parties.
For license and attribution notices for these materials, please refer to the documentation that accompanies this distribution (see the "Licenses for Third-Party Components" appendix) or view the online documentation at http://dev.mysql.com/doc/.

__GPLv2 Disclaimer__
For the avoidance of doubt, except that if any license choice other than GPL or LGPL is available it will apply instead, Oracle elects to use only the General Public License version 2 (GPLv2) at this time for any software where a choice of GPL  license versions is made available with the language indicating that GPLv2 or any later version may be used, or where a choice of which version of the GPL is applied is otherwise unspecified.