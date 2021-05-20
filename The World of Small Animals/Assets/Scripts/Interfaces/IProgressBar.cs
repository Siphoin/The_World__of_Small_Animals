using System.Collections;

public   interface IProgressBar
    {
    void UpdateProgress(float value);

    IEnumerator UpdateProgresAsync(float value);
    }