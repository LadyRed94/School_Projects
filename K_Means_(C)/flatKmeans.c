#include <stdio.h>
#include <stdlib.h>
#include <omp.h>
#include <math.h>

double RandomDouble (int* randCount){
    srand(*randCount++);
    return ((double)rand()/RAND_MAX)*100;
}
int main(void){
    const int count = 1000;
    const int k = 2;
    const double max = 100.0;
    double * arr = (double*)malloc(sizeof(double)*(2*count) + sizeof(double)*(2*k)); 

    int seed = 0;

    //Array layout = centroids, then points
    arr[0] = 0.0;
    arr[1] = 0.0;
    arr[2] = max;
    arr[3] = max;
    #pragma omp parallel for
    for (int i = k*2; i < (count*2) + (k*2); i++){
        //even odd set
        arr[i] = RandomDouble(&seed);
    }
    //Is point nearer to centroid?    
    //Recalc the centroid based off that at the same time.

    int isFinal = 0;
    int runCount = 0;
    while(isFinal != 1 && runCount < 15){
        //determine which centroid is closer'
        runCount++;
        printf("%d\n", runCount);
        double x1Adjust = 0.0; //first centroid
        double y1Adjust = 0.0;
        double x2Adjust = 0.0; //second centroid
        double y2Adjust = 0.0;
        #pragma omp parallel reduction (+:x1Adjust,x2Adjust,y1Adjust,y2Adjust)
        for (int i = k; i < (count*2) + (k*2); i+=2){ //iterate the points
            double dist1 = sqrt ( (pow ( (arr[0] - arr[i] ),2 ) + pow((arr[1] - arr[i+1]), 2))); //distance from point to centroid 1
            double dist2 = sqrt ( (pow ( (arr[2] - arr[i] ),2 ) + pow((arr[3] - arr[i+1]), 2))); //distance from point to centroid 2
            //adjust the centroid correct centroid based off which is closer
            //printf("%d\n", i);
            if(dist1 < dist2){ //dist 1 is closer, so we adjust it by a small amount
                //amount
                x1Adjust += (arr[i] / count); //we dont work with negative values, so fabs is probably not required
                y1Adjust += (arr[i+1] / count);
            }
            else {
                x2Adjust += (arr[i] / count);
                y2Adjust += (arr[i+1] / count);
            }

        }
        //printf("after for loop\n");
        double lastCentroidX1 = arr[0];
        double lastCentroidY1 = arr[1];
        double lastCentroidX2 = arr[2];
        double lastCentroidY2 = arr[3];
        if(x1Adjust == lastCentroidX1 && y1Adjust == lastCentroidY1 && x2Adjust == lastCentroidX2 && y2Adjust == lastCentroidY2){
            printf("FINAL: cent1: (%g, %g), cent2: (%g, %g)\n", x1Adjust, y1Adjust, x2Adjust, y2Adjust);
            isFinal = 1;
        }
        else if (x2Adjust == lastCentroidX1 && y2Adjust == lastCentroidY1 && x1Adjust == lastCentroidX2 && y1Adjust == lastCentroidY2) {//check for the flipped condition. May not be required.  
            printf("FLIPPED cent1: (%g, %g), cent2: (%g, %g)\n", x1Adjust, y1Adjust, x2Adjust, y2Adjust);
            isFinal = 1;       
        }
        else{ //update the centroids with the new locations
            printf("cent 1(%g, %g) => (%g,%g)\n", arr[0], arr[1], x1Adjust, y1Adjust);
            printf("cent 2(%g, %g) => (%g,%g)\n", arr[2], arr[3], x2Adjust, y2Adjust);
            arr[0] = x1Adjust;
            arr[1] = y1Adjust;
            arr[2] = x2Adjust;
            arr[3] = y2Adjust;
        }
    }
}