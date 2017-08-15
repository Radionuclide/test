#pragma once

namespace S7_writer
{

	public ref class S7ConnectionParameters
    {
    public:
		S7ConnectionParameters()
		{
			Address = "192.168.123.205";
			Rack = 0;
			Slot = 2;
			ConnType = 0;
			TimeoutInSec = 10;
		}

		String^ Address;
		int Rack;
		int Slot;
		int ConnType;
		int TimeoutInSec;
    };

}//end namespace S7_writer
