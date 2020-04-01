﻿using System;

namespace DungeonCrawlers.UI
{
	public interface IClosable
	{
		event EventHandler Closed;
		void Close();
	}
}