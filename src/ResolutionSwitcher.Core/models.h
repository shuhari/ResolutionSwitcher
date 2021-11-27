#pragma once


namespace ResolutionSwitcher {

    using namespace System;
    using namespace System::Drawing;

    // DEVICE_SCALE_FACTOR (shtypes.h)
	public enum class DeviceScaleFactor {
        Invalid = 0,
        Scale100 = 100,
        Scale120 = 120,
        Scale125 = 125,
        Scale140 = 140,
        Scale150 = 150,
        Scale160 = 160,
        Scale175 = 175,
        Scale180 = 180,
        Scale200 = 200,
        Scale225 = 225,
        Scale250 = 250,
        Scale300 = 300,
        Scale350 = 350,
        Scale400 = 400,
        Scale450 = 450,
        Scale500 = 500,
	};

    public enum class DisplayOrientation {
        Default = 0,
        Rotate90 = 1,
        Rotate180 = 2,
        Rotate270 = 3,
    };

    public ref class Names {
    public:
        inline static String^ OfResolution(Size resolution) {
            return String::Format("{0} * {1}", resolution.Width, resolution.Height);
        }

        inline static String^ OfOrientation(DisplayOrientation orientation) {
            switch (orientation) {
            case DisplayOrientation::Default: return "No Rotate";
            case DisplayOrientation::Rotate90: return "Rotate 90";
            case DisplayOrientation::Rotate180: return "Rotate 180";
            case DisplayOrientation::Rotate270: return "Rotate 270";
            default: return "";
            }
        }

        inline static String^ OfScale(DeviceScaleFactor scale) {
            if ((int)scale > 0) {
                return String::Format("{0}%", (int)scale);
            }
            return "";
        }
    };

    public ref class DisplayMode {
    public:
        inline DisplayMode(Size resolution, DisplayOrientation orientation, DeviceScaleFactor scale) {
            _resolution = resolution;
            _orientation = orientation;
            _scale = scale;
        }

        property Size Resolution { inline Size get() { return _resolution; } }
        property DisplayOrientation Orientation { inline DisplayOrientation get() { return _orientation; }}
        property DeviceScaleFactor Scale { inline DeviceScaleFactor get() { return _scale; }}

        inline String^ ToString() override {
            return String::Format("{0}, {1}, {2}", 
                Names::OfResolution(_resolution),
                Names::OfOrientation(_orientation),
                Names::OfScale(_scale));
        }

        inline int GetHashCode() override { return _resolution.GetHashCode(); }

        inline bool Equals(Object^ obj) override {
            auto other = dynamic_cast<DisplayMode^>(obj);
            if (other != nullptr) {
                return this->Resolution == other->Resolution &&
                    this->Orientation == other->Orientation &&
                    this->Scale == other->Scale;
            }
            return false;
        }

    private:
        Size _resolution;
        DisplayOrientation _orientation;
        DeviceScaleFactor _scale;
    };

    public ref class DisplayApi {
    public:
        static property DeviceScaleFactor RecommendedDPIScaling{
            DeviceScaleFactor get();
            void set(DeviceScaleFactor value);
        }
        static array<Size>^ GetResolutions();
        static Size GetRecommendedResolution();

        static DisplayMode^ GetRecommendedMode();
        static DisplayMode^ GetCurrentMode();
        static void SetCurrentMode(DisplayMode^ mode);
    };

}; // namespace ResolutionSwitcher