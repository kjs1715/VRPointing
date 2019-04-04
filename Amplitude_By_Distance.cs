public class Amplitude_By_Distance{

	public static float getAmplitude(float distance, float scale)
    {
        if(distance<=scale/2)
        {
            return 200;
        }
        else
        {
            float d = distance -scale / 2;
            return 100.0f / (1 + d * d);
        }
    }
}
