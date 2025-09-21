#ifndef EDGEDETECTION_INCLUDED
#define EDGEDETECTION_INCLUDED

static float2 nbhd[9] = {
    float2(-1,1), float2(0,1), float2(1,1),
    float2(-1,0), float2(0,0), float2(1,1),
    float2(-1,-1), float2(0,-1), float2(1,-1),
};
static float sobelX[9] = {
    1,0,-1,
    2,0,-2,
    1,0,-1
};

static float sobelY[9] = {
    1,2,1,
    0,0,0,
    -1,-2,-1
};

void sobel_float(float2 UV, float Thickness, out float Out){
    float sum = 0;
    for (int i=1; i<9; i++){
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + nbhd[i]*Thickness);
        sum += depth * float2(sobelX[i], sobelY[i]);
    }

    Out = length(sum);
}

#endif