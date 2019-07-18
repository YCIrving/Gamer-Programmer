// leetcode 135

#include <iostream>
#include <algorithm>
#define MAX_INT 10010

using namespace std;

struct Student
{
    int index;
    int score;
    int candies;

}students_org[MAX_INT], students_sorted[MAX_INT];

bool cmp (Student s1, Student s2)
{
    return s1.score < s2.score;
}

int main()
{
    int n;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        students_org[i].index = i;
        students_org[i].candies = 0;
        cin>>students_org[i].score;
        students_sorted[i] = students_org[i];
    }

    sort(students_sorted, students_sorted+n, cmp);
    int output = 0;

    for(int i=0; i<n; i++)
    {
        Student student_temp = students_sorted[i];
        int id = student_temp.index;
        int idLeft = id -1, idRight = id + 1;
        int candy = 0;
        if (idLeft >=0 && students_org[idLeft].score < student_temp.score ) candy = students_org[idLeft].candies;
        if(idRight<n && students_org[idRight].score < student_temp.score && students_org[idRight].candies > candy) candy = students_org[idRight].candies;
        students_org[id].candies = candy + 1;
        output += students_org[id].candies;
        // cout<<id<<' '<<candy + 1<<endl;

    }
    cout<<output<<endl;
    return 0;
}
