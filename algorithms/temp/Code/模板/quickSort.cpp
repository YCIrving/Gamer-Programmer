#include <iostream>
using namespace std;

#define MAXN 1005
int a[MAXN];

void quickSort(int low, int high);
int partition(int low, int high);

int main() {

	int t,n;
	cin>>t;
	for(int tt=0; tt<t; tt++)
	{
	    cin>>n;
	    for(int i=0; i<n; i++)
	    {
	        cin>>a[i];
	    }
	    quickSort(0, n-1);
	    for(int i=0; i<n; i++)
	    {
	        cout<<a[i]<<' ';
	    }
	    cout<<endl;
	}
	return 0;
}

void quickSort(int low, int high)
{
    if(low < high)
    {
        int pi = partition(low, high);
        quickSort(low, pi-1); //易错点1：这里不对pi再进行排序
        quickSort(pi+1, high);
    }
}

int partition(int low, int high)
{
    int pivot = a[high];
    int temp;
    int i=low;
    for(int j = low; j<high; j++)
    {
        if(a[j] < pivot)
        {
            temp = a[i];
            a[i] = a[j];
            a[j] = temp;
            i++; //易错点2：容易忘记修改i的值
        }
    }
    temp = a[i];
    a[i] = a[high];
    a[high] = temp;
    return i; //最终i为轴所在的位置

}
