// AzureDKSDKExample.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <k4a/k4a.h>
#include <k4arecord/record.h>
#include <k4arecord/playback.h>
#ifdef _WIN32
#include <windows.h>
#else
#include <unistd.h>
#endif // _WIN32

void sleepcp(int milliseconds) // Cross-platform sleep function
{
#ifdef _WIN32
    Sleep(milliseconds);
#else
    usleep(milliseconds * 1000);
#endif // _WIN32
}

#define VERIFY(result)                                                            \
    if (result != K4A_RESULT_SUCCEEDED)                                                                                \
    {                                                                                                                  \
        printf("%s \n - (File: %s, Function: %s, Line: %d)\n", #result " failed", __FILE__, __FUNCTION__, __LINE__);   \
        exit(1);                                                                                                       \
    }

#define FOURCC(cc) ((cc)[0] | (cc)[1] << 8 | (cc)[2] << 16 | (cc)[3] << 24)

// Codec context struct for Codec ID: "V_MS/VFW/FOURCC"
// See https://docs.microsoft.com/en-us/windows/desktop/wmdm/-bitmapinfoheader
/*typedef struct
{
    uint32_t biSize;
    uint32_t biWidth;
    uint32_t biHeight;
    uint16_t biPlanes;
    uint16_t biBitCount;
    uint32_t biCompression;
    uint32_t biSizeImage;
    uint32_t biXPelsPerMeter;
    uint32_t biYPelsPerMeter;
    uint32_t biClrUsed;
    uint32_t biClrImportant;
} BITMAPINFOHEADER;*/

void fill_bitmap_header(uint32_t width, uint32_t height, BITMAPINFOHEADER* out);
void fill_bitmap_header(uint32_t width, uint32_t height, BITMAPINFOHEADER* out)
{
    out->biSize = sizeof(BITMAPINFOHEADER);
    out->biWidth = width;
    out->biHeight = height;
    out->biPlanes = 1;
    out->biBitCount = 16;
    out->biCompression = FOURCC("YUY2");
    out->biSizeImage = sizeof(uint16_t) * width * height;
    out->biXPelsPerMeter = 0;
    out->biYPelsPerMeter = 0;
    out->biClrUsed = 0;
    out->biClrImportant = 0;
}

//#define MULTI_DEVICES
//#define SINGLE_DEVICES
//#define IMAGE_RETRIEVE
#define RECORD

int main(int argc, char** argv)
{
#ifdef SINGLE_DEVICES
    uint32_t count = k4a_device_get_installed_count();
    if (count == 0)
    {
        printf("No k4a devices attached!\n");
        return 1;
    }
    std::cout << count <<" Kinects are connected"<< "\n";
    // Open the first plugged in Kinect device
    k4a_device_t device = NULL;
    if (K4A_FAILED(k4a_device_open(K4A_DEVICE_DEFAULT, &device)))
    {
        printf("Failed to open k4a device!\n");
        return 0;
    }
    // Get the size of the serial number
    size_t serial_size = 0;
    k4a_device_get_serialnum(device, NULL, &serial_size);

    // Allocate memory for the serial, then acquire it
    char* serial = (char*)(malloc(serial_size));
    k4a_device_get_serialnum(device, serial, &serial_size);
    printf("Opened device: %s\n", serial);
    free(serial);

    // Configure a stream of 4096x3072 BRGA color data at 15 frames per second
    k4a_device_configuration_t config = K4A_DEVICE_CONFIG_INIT_DISABLE_ALL;
    config.camera_fps = K4A_FRAMES_PER_SECOND_15;
    config.color_format = K4A_IMAGE_FORMAT_COLOR_BGRA32;
    config.color_resolution = K4A_COLOR_RESOLUTION_3072P;

    // Start the camera with the given configuration
    if (K4A_FAILED(k4a_device_start_cameras(device, &config)))
    {
        printf("Failed to start cameras!\n");
        k4a_device_close(device);
        return 1;
    }


    sleepcp(3000);
    // ...Camera capture and application specific code would go here...

    // Shut down the camera when finished with application logic
    k4a_device_stop_cameras(device);
    k4a_device_close(device);
    return 0;

#endif

#ifdef MULTI_DEVICES
    k4a_device_t device = NULL;
    uint32_t device_count = k4a_device_get_installed_count();
    printf("Found %d connected devices:\n", device_count);

    if (device_count != 1)
    {
        printf("Unexpected number of devices found (%d)\n", device_count);
        //goto Exit;
    }

    for (uint8_t deviceIndex = 0; deviceIndex < device_count; deviceIndex++)
    {
        
        if (K4A_RESULT_SUCCEEDED != k4a_device_open(deviceIndex, &device))
        {
            printf("%d: Failed to open device\n", deviceIndex);
            continue;      
        }
        printf("device number is %d\n", device->_rsvd);
        char* serial_number = NULL;
        size_t serial_number_length = 0;

        if (K4A_BUFFER_RESULT_TOO_SMALL != k4a_device_get_serialnum(device, NULL, &serial_number_length))
        {
            printf("%d: Failed to get serial number length\n", deviceIndex);
            k4a_device_close(device);
            device = NULL;
            continue;
        }

        serial_number = (char*)malloc(serial_number_length);
        if (serial_number == NULL)
        {
            printf("%d: Failed to allocate memory for serial number (%zu bytes)\n", deviceIndex, serial_number_length);
            k4a_device_close(device);
            device = NULL;
            continue;
        }

        if (K4A_BUFFER_RESULT_SUCCEEDED != k4a_device_get_serialnum(device, serial_number, &serial_number_length))
        {
            printf("%d: Failed to get serial number\n", deviceIndex);
            free(serial_number);
            serial_number = NULL;
            k4a_device_close(device);
            device = NULL;
            continue;
        }

        printf("%d: Device \"%s\"\n\n", deviceIndex, serial_number);
        k4a_device_close(device);
    }
#endif

#ifdef IMAGE_RETRIEVE
    k4a_device_t device = NULL;
    if (K4A_FAILED(k4a_device_open(K4A_DEVICE_DEFAULT, &device)))
    {
        printf("Failed to open k4a device!\n");
        return 0;
    }

    k4a_device_configuration_t config = K4A_DEVICE_CONFIG_INIT_DISABLE_ALL;
    config.camera_fps = K4A_FRAMES_PER_SECOND_30;
    config.color_format = K4A_IMAGE_FORMAT_COLOR_MJPG;
    config.color_resolution = K4A_COLOR_RESOLUTION_2160P;
    config.depth_mode = K4A_DEPTH_MODE_OFF;

    if (K4A_RESULT_SUCCEEDED != k4a_device_start_cameras(device, &config))
    {
        printf("Failed to start device\n");
        k4a_device_close(device);
        return 0;
    }
    int timeout_in_ms = 5000;
    k4a_capture_t capture;
    // Capture a depth frame
    switch (k4a_device_get_capture(device, &capture, timeout_in_ms))
    {
    case K4A_WAIT_RESULT_SUCCEEDED:
        break;
    case K4A_WAIT_RESULT_TIMEOUT:
        printf("Timed out waiting for a capture\n");
        //continue;
        break;
    case K4A_WAIT_RESULT_FAILED:
        printf("Failed to read a capture\n");
        k4a_device_stop_cameras(device);
        k4a_device_close(device);
        return 0;
    }

    // Access the rbg image
    k4a_image_t image = k4a_capture_get_color_image(capture);
    if (image != NULL)
    {
        printf(" | rgb res:%4dx%4d stride:%5d\n",
            k4a_image_get_height_pixels(image),
            k4a_image_get_width_pixels(image),
            k4a_image_get_stride_bytes(image));

        // Release the image, which must be called
        k4a_image_release(image);
    }

    // Release the capture
    k4a_capture_release(capture);


    k4a_device_stop_cameras(device);
    k4a_device_close(device);
#endif // IMAGE_RETRIEVE
#ifdef RECORD
    if (argc < 2)
    {
        printf("Usage: k4arecord_custom_track output.mkv\n");
        exit(0);
    }

    uint32_t device_count = k4a_device_get_installed_count();
    printf("Found %d connected devices:\n", device_count);

    char* recording_filename = argv[1];

    k4a_device_t device = NULL;
    if (K4A_FAILED(k4a_device_open(K4A_DEVICE_DEFAULT, &device)))
    {
        printf("Failed to open k4a device!\n");
        return 0;
    }

    k4a_device_configuration_t config = K4A_DEVICE_CONFIG_INIT_DISABLE_ALL;
    config.camera_fps = K4A_FRAMES_PER_SECOND_30;
    config.color_format = K4A_IMAGE_FORMAT_COLOR_MJPG;
    config.color_resolution = K4A_COLOR_RESOLUTION_2160P;
    config.depth_mode = K4A_DEPTH_MODE_OFF;

    if (K4A_RESULT_SUCCEEDED != k4a_device_start_cameras(device, &config))
    {
        printf("Failed to start device\n");
        k4a_device_close(device);
        return 0;
    }

    printf("Camera started\n");

    k4a_record_t recording;
    if (K4A_RESULT_SUCCEEDED != k4a_record_create(recording_filename, device, config, &recording))
    {
        printf("Unable to create recording file: %s\n", recording_filename);
        return 1;
    }

    // Add a hello_world.txt attachment to the recording
    const char* attachment_data = "Hello, World!\n";
    VERIFY(k4a_record_add_attachment(recording,
        "hello_world.txt",
        (const uint8_t*)attachment_data,
        strlen(attachment_data)));
    // Add a custom recording tag
    VERIFY(k4a_record_add_tag(recording, "CUSTOM_TAG", "Hello, World!"));


    // Add a custom video track to store processed rbg images.
    // Read the resolution from the camera configuration so we can create our custom track with the same size.
    k4a_calibration_t sensor_calibration;
    VERIFY(k4a_device_get_calibration(device, config.depth_mode, K4A_COLOR_RESOLUTION_2160P, &sensor_calibration));
    uint32_t color_width = (uint32_t)sensor_calibration.color_camera_calibration.resolution_width;
    uint32_t color_height = (uint32_t)sensor_calibration.color_camera_calibration.resolution_height;


    BITMAPINFOHEADER codec_header;
    fill_bitmap_header(color_width, color_height, &codec_header);

    k4a_record_video_settings_t video_settings;
    video_settings.width = color_width;
    video_settings.height = color_height;
    video_settings.frame_rate = 30; // Should be the same rate as device_config.camera_fps

    // Add the video track to the recording.
    VERIFY(k4a_record_add_custom_video_track(recording,
        "PROCESSED_COLOR",
        "V_MS/VFW/FOURCC",
        (uint8_t*)(&codec_header),
        sizeof(codec_header),
        &video_settings));

    // Write the recording header now that all the track metadata is set up.
    VERIFY(k4a_record_write_header(recording));

    // Start reading 100 color frames (~3 seconds at 30 fps) from the camera.
    for (int frame = 0; frame < 300; frame++)
    {
        k4a_capture_t capture;
        k4a_wait_result_t get_capture_result = k4a_device_get_capture(device, &capture, K4A_WAIT_INFINITE);
        if (get_capture_result == K4A_WAIT_RESULT_SUCCEEDED)
        {
            // Write the capture to the built-in tracks
            VERIFY(k4a_record_write_capture(recording, capture));

            // Get the color image from the capture so we can write a processed copy to our custom track.
#pragma region optional codes
            k4a_image_t color_image = k4a_capture_get_color_image(capture);
            if (color_image)
            {
                // The YUY2 image format is the same stride as the 16-bit depth image, so we can modify it in-place.
                uint8_t* color_buffer = k4a_image_get_buffer(color_image);
                size_t color_buffer_size = k4a_image_get_size(color_image);
                //for (size_t i = 0; i < color_buffer_size; i += 2)
                //{
                //    // Convert the depth value (16-bit, in millimeters) to the YUY2 color format.
                //    // The YUY2 format should be playable in video players such as VLC.
                //    uint16_t depth = (uint16_t)(color_buffer[i + 1] << 8 | color_buffer[i]);
                //    // Clamp the depth range to ~1 meter and scale it to fit in the Y channel of the image (8-bits).
                //    if (depth > 0x3FF)
                //    {
                //        color_buffer[i] = 0xFF;
                //    }
                //    else
                //    {
                //        color_buffer[i] = (uint8_t)(depth >> 2);
                //    }
                //    // Set the U/V channel to 128 (i.e. grayscale).
                //    color_buffer[i + 1] = 128;
                //}

                VERIFY(k4a_record_write_custom_track_data(recording,
                    "PROCESSED_COLOR",
                    k4a_image_get_device_timestamp_usec(color_image),
                    color_buffer,
                    (uint32_t)color_buffer_size));

                k4a_image_release(color_image);
            }
#pragma endregion

            k4a_capture_release(capture);
        }
        else if (get_capture_result == K4A_WAIT_RESULT_TIMEOUT)
        {
            // TIMEOUT should never be returned when K4A_WAIT_INFINITE is set.
            printf("k4a_device_get_capture() timed out!\n");
            break;
        }
        else
        {
            printf("k4a_device_get_capture() returned error: %d\n", get_capture_result);
            break;
        }
    }

    k4a_device_stop_cameras(device);

    printf("Saving recording...\n");
    VERIFY(k4a_record_flush(recording));
    k4a_record_close(recording);

    printf("Done\n");
    k4a_device_close(device);

    return 0;








    int timeout_in_ms = 5000;
    k4a_capture_t capture;




    // Capture a depth frame
    switch (k4a_device_get_capture(device, &capture, timeout_in_ms))
    {
    case K4A_WAIT_RESULT_SUCCEEDED:
        break;
    case K4A_WAIT_RESULT_TIMEOUT:
        printf("Timed out waiting for a capture\n");
        //continue;
        break;
    case K4A_WAIT_RESULT_FAILED:
        printf("Failed to read a capture\n");
        k4a_device_stop_cameras(device);
        k4a_device_close(device);
        return 0;
    }

    // Access the rbg image
    k4a_image_t image = k4a_capture_get_color_image(capture);
    if (image != NULL)
    {
        printf(" | rgb res:%4dx%4d stride:%5d\n",
            k4a_image_get_height_pixels(image),
            k4a_image_get_width_pixels(image),
            k4a_image_get_stride_bytes(image));

        // Release the image, which must be called
        k4a_image_release(image);
    }

    // Release the capture
    k4a_capture_release(capture);


    k4a_device_stop_cameras(device);
    k4a_device_close(device);
#endif // RECORD_AND_PLAYBACK

Exit: return 0;
    exit(0);
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
