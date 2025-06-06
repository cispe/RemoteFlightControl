using RemoteFlightControl.MqttClient.MSP.Requests;
using RemoteFlightControl.MqttClient.MSP.Responses;

namespace RemoteFlightControl.MqttClient.MSP;

/// <summary>
/// Enumerates all Multiwii Serial Protocol (MSP) commands supported by the flight controller and related systems.
/// Each value represents a specific command that can be sent or received, enabling configuration, telemetry, and control operations
/// </summary>
public enum MspCommand : byte
{
    // Protocol versioning and identifiers
    /// <summary>
    /// Get API version
    /// </summary>
    API_VERSION = 1,

    /// <summary>
    /// Get flight controller variant
    /// </summary>
    FC_VARIANT = 2,

    /// <summary>
    /// Get flight controller firmware version
    /// </summary>
    FC_VERSION = 3,

    /// <summary>
    /// Get board information
    /// </summary>
    BOARD_INFO = 4,

    /// <summary>
    /// Get build information
    /// </summary>
    BUILD_INFO = 5,


    /// <summary>
    /// Get craft name
    /// </summary>
    NAME = 10,

    /// <summary>
    /// Set craft name
    /// </summary>
    SET_NAME = 11,


    // Cleanflight original features (32-62)
    /// <summary>
    /// Get battery configuration
    /// </summary>
    BATTERY_CONFIG = 32,

    /// <summary>
    /// Set battery configuration
    /// </summary>
    SET_BATTERY_CONFIG = 33,

    /// <summary>
    /// Get mode ranges
    /// </summary>
    MODE_RANGES = 34,

    /// <summary>
    /// Set mode range
    /// </summary>
    SET_MODE_RANGE = 35,

    /// <summary>
    /// Get feature configuration
    /// </summary>
    FEATURE_CONFIG = 36,

    /// <summary>
    /// Set feature configuration
    /// </summary>
    SET_FEATURE_CONFIG = 37,

    /// <summary>
    /// Get board alignment configuration
    /// </summary>
    BOARD_ALIGNMENT_CONFIG = 38,

    /// <summary>
    /// Set board alignment configuration
    /// </summary>
    SET_BOARD_ALIGNMENT_CONFIG = 39,

    /// <summary>
    /// Get current meter configuration
    /// </summary>
    CURRENT_METER_CONFIG = 40,

    /// <summary>
    /// Set current meter configuration
    /// </summary>
    SET_CURRENT_METER_CONFIG = 41,

    /// <summary>
    /// Get mixer configuration
    /// </summary>
    MIXER_CONFIG = 42,

    /// <summary>
    /// Set mixer configuration
    /// </summary>
    SET_MIXER_CONFIG = 43,

    /// <summary>
    /// Get RX configuration
    /// </summary>
    RX_CONFIG = 44,

    /// <summary>
    /// Set RX configuration
    /// </summary>
    SET_RX_CONFIG = 45,

    /// <summary>
    /// Get LED colors
    /// </summary>
    LED_COLORS = 46,

    /// <summary>
    /// Set LED colors
    /// </summary>
    SET_LED_COLORS = 47,

    /// <summary>
    /// Get LED strip configuration
    /// </summary>
    LED_STRIP_CONFIG = 48,

    /// <summary>
    /// Set LED strip configuration
    /// </summary>
    SET_LED_STRIP_CONFIG = 49,

    /// <summary>
    /// Get RSSI configuration
    /// </summary>
    RSSI_CONFIG = 50,

    /// <summary>
    /// Set RSSI configuration
    /// </summary>
    SET_RSSI_CONFIG = 51,

    /// <summary>
    /// Get adjustment ranges
    /// </summary>
    ADJUSTMENT_RANGES = 52,

    /// <summary>
    /// Set adjustment range
    /// </summary>
    SET_ADJUSTMENT_RANGE = 53,

    /// <summary>
    /// Get CF serial configuration
    /// </summary>
    CF_SERIAL_CONFIG = 54,

    /// <summary>
    /// Set CF serial configuration
    /// </summary>
    SET_CF_SERIAL_CONFIG = 55,

    /// <summary>
    /// Get voltage meter configuration
    /// </summary>
    VOLTAGE_METER_CONFIG = 56,

    /// <summary>
    /// Set voltage meter configuration
    /// </summary>
    SET_VOLTAGE_METER_CONFIG = 57,

    /// <summary>
    /// Get sonar altitude
    /// </summary>
    SONAR_ALTITUDE = 58,

    /// <summary>
    /// Get PID controller
    /// </summary>
    PID_CONTROLLER = 59,

    /// <summary>
    /// Set PID controller
    /// </summary>
    SET_PID_CONTROLLER = 60,

    /// <summary>
    /// Get arming configuration
    /// </summary>
    ARMING_CONFIG = 61,

    /// <summary>
    /// Set arming configuration
    /// </summary>
    SET_ARMING_CONFIG = 62,


    // Baseflight MSP commands (64-89)
    /// <summary>
    /// Get RX map
    /// </summary>
    RX_MAP = 64,

    /// <summary>
    /// Set RX map
    /// </summary>
    SET_RX_MAP = 65,

    /// <summary>
    /// Reboot flight controller
    /// </summary>
    REBOOT = 68,

    /// <summary>
    /// Get dataflash summary
    /// </summary>
    DATAFLASH_SUMMARY = 70,

    /// <summary>
    /// Read dataflash
    /// </summary>
    DATAFLASH_READ = 71,

    /// <summary>
    /// Erase dataflash
    /// </summary>
    DATAFLASH_ERASE = 72,

    /// <summary>
    /// Get failsafe configuration
    /// </summary>
    FAILSAFE_CONFIG = 75,

    /// <summary>
    /// Set failsafe configuration
    /// </summary>
    SET_FAILSAFE_CONFIG = 76,

    /// <summary>
    /// Get RX fail configuration
    /// </summary>
    RXFAIL_CONFIG = 77,

    /// <summary>
    /// Set RX fail configuration
    /// </summary>
    SET_RXFAIL_CONFIG = 78,

    /// <summary>
    /// Get SD card summary
    /// </summary>
    SDCARD_SUMMARY = 79,

    /// <summary>
    /// Get blackbox configuration
    /// </summary>
    BLACKBOX_CONFIG = 80,

    /// <summary>
    /// Set blackbox configuration
    /// </summary>
    SET_BLACKBOX_CONFIG = 81,

    /// <summary>
    /// Get transponder configuration
    /// </summary>
    TRANSPONDER_CONFIG = 82,

    /// <summary>
    /// Set transponder configuration
    /// </summary>
    SET_TRANSPONDER_CONFIG = 83,

    /// <summary>
    /// Get OSD configuration
    /// </summary>
    OSD_CONFIG = 84,

    /// <summary>
    /// Set OSD configuration
    /// </summary>
    SET_OSD_CONFIG = 85,

    /// <summary>
    /// Read OSD character
    /// </summary>
    OSD_CHAR_READ = 86,

    /// <summary>
    /// Write OSD character
    /// </summary>
    OSD_CHAR_WRITE = 87,

    /// <summary>
    /// Get VTX configuration
    /// </summary>
    VTX_CONFIG = 88,

    /// <summary>
    /// Set VTX configuration
    /// </summary>
    SET_VTX_CONFIG = 89,

    // Betaflight Additional Commands (90-99)
    /// <summary>
    /// Get advanced configuration
    /// </summary>
    ADVANCED_CONFIG = 90,

    /// <summary>
    /// Set advanced configuration
    /// </summary>
    SET_ADVANCED_CONFIG = 91,

    /// <summary>
    /// Get filter configuration
    /// </summary>
    FILTER_CONFIG = 92,

    /// <summary>
    /// Set filter configuration
    /// </summary>
    SET_FILTER_CONFIG = 93,

    /// <summary>
    /// Get advanced PID settings
    /// </summary>
    PID_ADVANCED = 94,

    /// <summary>
    /// Set advanced PID settings
    /// </summary>
    SET_PID_ADVANCED = 95,

    /// <summary>
    /// Get sensor configuration
    /// </summary>
    SENSOR_CONFIG = 96,

    /// <summary>
    /// Set sensor configuration
    /// </summary>
    SET_SENSOR_CONFIG = 97,

    /// <summary>
    /// Control camera
    /// </summary>
    CAMERA_CONTROL = 98,

    /// <summary>
    /// Set arming disabled
    /// </summary>
    SET_ARMING_DISABLED = 99,

    // Multiwii original MSP commands (101-139)
    /// <summary>
    /// Get status
    /// </summary>
    STATUS = 101,

    /// <summary>
    /// Get raw IMU data
    /// </summary>
    RAW_IMU = 102,

    /// <summary>
    /// Get servo data
    /// </summary>
    SERVO = 103,

    /// <summary>
    /// Get motor data
    /// </summary>
    MOTOR = 104,

    /// <summary>
    /// Get RC data
    /// </summary>
    RC = 105,

    /// <summary>
    /// Get raw GPS data
    /// </summary>
    RAW_GPS = 106,

    /// <summary>
    /// Get computed GPS data
    /// </summary>
    COMP_GPS = 107,

    /// <summary>
    /// Get current attitude (roll, pitch, yaw). Request: <see cref="MspRequest_Attitude"/>; Response: <see cref="MspResponse_Attitude"/>
    /// </summary>
    ATTITUDE = 108,

    /// <summary>
    /// Get altitude
    /// </summary>
    ALTITUDE = 109,

    /// <summary>
    /// Get analog values
    /// </summary>
    ANALOG = 110,

    /// <summary>
    /// Get RC tuning
    /// </summary>
    RC_TUNING = 111,

    /// <summary>
    /// Get PID values
    /// </summary>
    PID = 112,

    /// <summary>
    /// Get box names
    /// </summary>
    BOXNAMES = 116,

    /// <summary>
    /// Get PID names
    /// </summary>
    PIDNAMES = 117,

    /// <summary>
    /// Get waypoint
    /// </summary>
    WP = 118,

    /// <summary>
    /// Get box IDs
    /// </summary>
    BOXIDS = 119,

    /// <summary>
    /// Get servo configurations
    /// </summary>
    SERVO_CONFIGURATIONS = 120,

    /// <summary>
    /// Get navigation status
    /// </summary>
    NAV_STATUS = 121,

    /// <summary>
    /// Get navigation configuration
    /// </summary>
    NAV_CONFIG = 122,

    /// <summary>
    /// Get 3D motor configuration
    /// </summary>
    MOTOR_3D_CONFIG = 124,

    /// <summary>
    /// Get RC deadband
    /// </summary>
    RC_DEADBAND = 125,

    /// <summary>
    /// Get sensor alignment
    /// </summary>
    SENSOR_ALIGNMENT = 126,

    /// <summary>
    /// Get LED strip mode color
    /// </summary>
    LED_STRIP_MODECOLOR = 127,

    /// <summary>
    /// Get voltage meters
    /// </summary>
    VOLTAGE_METERS = 128,

    /// <summary>
    /// Get current meters
    /// </summary>
    CURRENT_METERS = 129,

    /// <summary>
    /// Get battery state
    /// </summary>
    BATTERY_STATE = 130,

    /// <summary>
    /// Get motor configuration
    /// </summary>
    MOTOR_CONFIG = 131,

    /// <summary>
    /// Get GPS configuration
    /// </summary>
    GPS_CONFIG = 132,

    /// <summary>
    /// Get compass configuration
    /// </summary>
    COMPASS_CONFIG = 133,

    /// <summary>
    /// Get ESC sensor data
    /// </summary>
    ESC_SENSOR_DATA = 134,

    /// <summary>
    /// Get GPS rescue configuration
    /// </summary>
    GPS_RESCUE = 135,

    /// <summary>
    /// Get GPS rescue PIDs
    /// </summary>
    GPS_RESCUE_PIDS = 136,

    /// <summary>
    /// Get VTX table band
    /// </summary>
    VTXTABLE_BAND = 137,

    /// <summary>
    /// Get VTX table power level
    /// </summary>
    VTXTABLE_POWERLEVEL = 138,

    /// <summary>
    /// Get motor telemetry
    /// </summary>
    MOTOR_TELEMETRY = 139,

    // Simplified tuning commands (140-145)
    /// <summary>
    /// Get simplified tuning
    /// </summary>
    SIMPLIFIED_TUNING = 140,

    /// <summary>
    /// Set simplified tuning
    /// </summary>
    SET_SIMPLIFIED_TUNING = 141,

    /// <summary>
    /// Calculate simplified PID
    /// </summary>
    CALCULATE_SIMPLIFIED_PID = 142,

    /// <summary>
    /// Calculate simplified gyro
    /// </summary>
    CALCULATE_SIMPLIFIED_GYRO = 143,

    /// <summary>
    /// Calculate simplified D-term
    /// </summary>
    CALCULATE_SIMPLIFIED_DTERM = 144,

    /// <summary>
    /// Validate simplified tuning
    /// </summary>
    VALIDATE_SIMPLIFIED_TUNING = 145,


    // Additional non-MultiWii commands (150-166)
    /// <summary>
    /// Get extended status
    /// </summary>
    STATUS_EX = 150,

    /// <summary>
    /// Get unique identifier
    /// </summary>
    UID = 160,

    /// <summary>
    /// Get GPS SV info
    /// </summary>
    GPSSVINFO = 164,

    /// <summary>
    /// Get GPS statistics
    /// </summary>
    GPSSTATISTICS = 166,


    // OSD specific commands (180-189)
    /// <summary>
    /// Get OSD video configuration
    /// </summary>
    OSD_VIDEO_CONFIG = 180,

    /// <summary>
    /// Set OSD video configuration
    /// </summary>
    SET_OSD_VIDEO_CONFIG = 181,

    /// <summary>
    /// Display port operations
    /// </summary>
    DISPLAYPORT = 182,

    /// <summary>
    /// Copy profile
    /// </summary>
    COPY_PROFILE = 183,

    /// <summary>
    /// Get beeper configuration
    /// </summary>
    BEEPER_CONFIG = 184,

    /// <summary>
    /// Set beeper configuration
    /// </summary>
    SET_BEEPER_CONFIG = 185,

    /// <summary>
    /// Set TX info
    /// </summary>
    SET_TX_INFO = 186,

    /// <summary>
    /// Get TX info
    /// </summary>
    TX_INFO = 187,

    /// <summary>
    /// Set OSD canvas
    /// </summary>
    SET_OSD_CANVAS = 188,

    /// <summary>
    /// Get OSD canvas
    /// </summary>
    OSD_CANVAS = 189,


    // Set commands (200-229)
    /// <summary>
    /// Set raw RC values
    /// </summary>
    SET_RAW_RC = 200,

    /// <summary>
    /// Set raw GPS values
    /// </summary>
    SET_RAW_GPS = 201,

    /// <summary>
    /// Set PID values
    /// </summary>
    SET_PID = 202,

    /// <summary>
    /// Set RC tuning
    /// </summary>
    SET_RC_TUNING = 204,

    /// <summary>
    /// Start accelerometer calibration
    /// </summary>
    ACC_CALIBRATION = 205,

    /// <summary>
    /// Start magnetometer calibration
    /// </summary>
    MAG_CALIBRATION = 206,

    /// <summary>
    /// Reset configuration
    /// </summary>
    RESET_CONF = 208,

    /// <summary>
    /// Set waypoint
    /// </summary>
    SET_WP = 209,

    /// <summary>
    /// Select setting
    /// </summary>
    SELECT_SETTING = 210,

    /// <summary>
    /// Set heading
    /// </summary>
    SET_HEADING = 211,

    /// <summary>
    /// Set servo configuration
    /// </summary>
    SET_SERVO_CONFIGURATION = 212,

    /// <summary>
    /// Set motor values
    /// </summary>
    SET_MOTOR = 214,

    /// <summary>
    /// Set navigation configuration
    /// </summary>
    SET_NAV_CONFIG = 215,

    /// <summary>
    /// Set 3D motor configuration
    /// </summary>
    SET_MOTOR_3D_CONFIG = 217,

    /// <summary>
    /// Set RC deadband
    /// </summary>
    SET_RC_DEADBAND = 218,

    /// <summary>
    /// Set and reset current PID
    /// </summary>
    SET_RESET_CURR_PID = 219,

    /// <summary>
    /// Set sensor alignment
    /// </summary>
    SET_SENSOR_ALIGNMENT = 220,

    /// <summary>
    /// Set LED strip mode color
    /// </summary>
    SET_LED_STRIP_MODECOLOR = 221,

    /// <summary>
    /// Set motor configuration
    /// </summary>
    SET_MOTOR_CONFIG = 222,

    /// <summary>
    /// Set GPS configuration
    /// </summary>
    SET_GPS_CONFIG = 223,

    /// <summary>
    /// Set compass configuration
    /// </summary>
    SET_COMPASS_CONFIG = 224,

    /// <summary>
    /// Set GPS rescue configuration
    /// </summary>
    SET_GPS_RESCUE = 225,

    /// <summary>
    /// Set GPS rescue PIDs
    /// </summary>
    SET_GPS_RESCUE_PIDS = 226,

    /// <summary>
    /// Set VTX table band
    /// </summary>
    SET_VTXTABLE_BAND = 227,

    /// <summary>
    /// Set VTX table power level
    /// </summary>
    SET_VTXTABLE_POWERLEVEL = 228,


    // Multiple MSP and special commands (230-255)
    /// <summary>
    /// Send multiple MSP commands
    /// </summary>
    MULTIPLE_MSP = 230,

    /// <summary>
    /// Get extra mode ranges
    /// </summary>
    MODE_RANGES_EXTRA = 238,

    /// <summary>
    /// Set accelerometer trim
    /// </summary>
    SET_ACC_TRIM = 239,

    /// <summary>
    /// Get accelerometer trim
    /// </summary>
    ACC_TRIM = 240,

    /// <summary>
    /// Get servo mix rules
    /// </summary>
    SERVO_MIX_RULES = 241,

    /// <summary>
    /// Set servo mix rule
    /// </summary>
    SET_SERVO_MIX_RULE = 242,

    /// <summary>
    /// Set passthrough
    /// </summary>
    SET_PASSTHROUGH = 245,

    /// <summary>
    /// Set real-time clock
    /// </summary>
    SET_RTC = 246,

    /// <summary>
    /// Get real-time clock
    /// </summary>
    RTC = 247,

    /// <summary>
    /// Set board information
    /// </summary>
    SET_BOARD_INFO = 248,

    /// <summary>
    /// Set signature
    /// </summary>
    SET_SIGNATURE = 249,

    /// <summary>
    /// Write EEPROM
    /// </summary>
    EEPROM_WRITE = 250,

    /// <summary>
    /// Reserved
    /// </summary>
    RESERVE_1 = 251,

    /// <summary>
    /// Reserved
    /// </summary>
    RESERVE_2 = 252,

    /// <summary>
    /// Get debug message
    /// </summary>
    DEBUGMSG = 253,

    /// <summary>
    /// Get debug data
    /// </summary>
    DEBUG = 254,

    /// <summary>
    /// V2 frame operations
    /// </summary>
    V2_FRAME = 255
}