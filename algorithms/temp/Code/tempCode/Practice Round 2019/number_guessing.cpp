//#include <iostream>
//#include <string>
//using namespace std;
//
//int main()
//{
//    int t;
//    int a, b, n;
//    int guess_num;
//    string out_str;
//
//    cin>>t;
//    for (int i=0; i<t; i++) {
//        cin>>a>>b;
//        cin>>n;
//        a++;
//        while(1)
//        {
//            guess_num = (a+b)/2;
//            cout<<guess_num<<endl;
//            cin>>out_str;
//            if(out_str == "CORRECT") {
//                break;
//            } else if (out_str == "TOO_SMALL") {
//                a = guess_num + 1;
//            } else if (out_str == "TOO_BIG") {
//                b = guess_num - 1;
//            } else {
//                return -1;
//            }
//        }
//    }
//    return 0;
//}

#include <iostream>
#include <string>

int main() {
  int num_test_cases;
  std::cin >> num_test_cases;
  for (int i = 0; i < num_test_cases; ++i) {
    int lo, hi;
    std::cin >> lo >> hi;
    int num_tries;
    std::cin >> num_tries;
    int head = lo + 1, tail = hi;
    while (true) {
      int m = (head + tail) / 2;
      std::cout << m << std::endl;
      std::string s;
      std::cin >> s;
      if (s == "CORRECT") break;
      if (s == "TOO_SMALL")
        head = m + 1;
      else
        tail = m - 1;
    }
  }
  return 0;
}
