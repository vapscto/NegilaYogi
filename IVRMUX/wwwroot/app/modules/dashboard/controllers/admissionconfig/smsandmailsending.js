//dashboard.controller("smsandmailsendingController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {

//    $scope.predicate = 'sno';
//    $scope.reverse = false;
//    $scope.currentPage = 1;
//    $scope.order = function (predicate) {
//        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
//        $scope.predicate = predicate;
//    };
//    $scope.students = [
//      { sno: '1', name: 'Kevin', age: 25, gender: 'boy' },
//      { sno: '2', name: 'John', age: 30, gender: 'girl' },
//      { sno: '3', name: 'Laura', age: 28, gender: 'girl' },
//      { sno: '4', name: 'Joy', age: 15, gender: 'girl' },
//      { sno: '5', name: 'Mary', age: 28, gender: 'girl' },
//      { sno: '6', name: 'Peter', age: 95, gender: 'boy' },
//      { sno: '7', name: 'Bob', age: 50, gender: 'boy' },
//      { sno: '8', name: 'Erika', age: 27, gender: 'girl' },
//      { sno: '9', name: 'Patrick', age: 40, gender: 'boy' },
//      { sno: '10', name: 'Tery', age: 60, gender: 'girl' }
//    ];
//    $scope.totalItems = $scope.students.length;
//    $scope.numPerPage = 5;
//    $scope.paginate = function (value) {
//        var begin, end, index;
//        begin = ($scope.currentPage - 1) * $scope.numPerPage;
//        end = begin + $scope.numPerPage;
//        index = $scope.students.indexOf(value);
//        return (begin <= index && index < end);
//    };


//}]);



(function () {
    'use strict';
    angular
.module('app')
.controller('SendingSMSandMAILSController', CustomeSendingSMSandMAILSController123)

    CustomeSendingSMSandMAILSController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CustomeSendingSMSandMAILSController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            apiService.getURI("SendingSMSandMAILS/getalldetails", pageid).
        then(function (promise) {
            $scope.classcount = promise.clslist;
            $scope.arrlist6 = promise.yerlist;         
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        $scope.GetstudentorstaffDetails = function () {


            var data = {
                "stustaffflag": $scope.rdovalue,
                "acayearid": $scope.ASMAYId,
                "acaclsid": $scope.ASMCLId
            }

            apiService.create("SendingSMSandMAILS/getdetailsstudentorstaff/", data).
        then(function (promise) {
            if ($scope.rdovalue == 'stu')
            {
                $scope.gridviewstudents = true;
                $scope.gridviewstaff = false;
                $scope.students = promise.fillstudent;
            }
            else if ($scope.rdovalue == 'staff') {
                $scope.gridviewstudents = false;
                $scope.gridviewstaff = true;
                $scope.staff = promise.fillstudent;
            }          
           
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
            
            
        //$scope.SendMSG = function (Content) {
        //    var Content = $scope.content
        //    //$scope.albumNameArray = [];
        //    //angular.forEach($scope.reportdetails, function (user) {
        //    //    if (!!user.selected) $scope.albumNameArray.push(user);
        //    //})
        //    if ($scope.mobileno.length > 0) {
        //        var data = {                  
        //            "smsmobileno": $scope.mobileno,
        //            "smscontent": $scope.content
        //        };
        //        apiService.create("CustomeSendingSMSandMAILS/SendSms/", data).then(function (promise) {
        //            swal("SMS Sent Successfully");
        //            $scope.subject = '';
        //            $scope.mobileno = '';
        //            $scope.content = '';
        //        });
        //    }
        //    else {
        //        swal("Please select atleat one record");
        //    }
        //};

        $scope.SendMAIL = function (Contentx) {
            var Contentx = $scope.content
            $scope.myArray = [];
            if ($scope.rdovalue == 'gen')
            {
                angular.forEach($scope.personalDetails, function (user) {
                    if (!!user.selected) $scope.myArray.push(user);
                })
            }
            else if ($scope.rdovalue == 'stu')
            {
                angular.forEach($scope.students, function (user) {
                    if (!!user.selected) $scope.myArray.push(user);
                })

            }
            else if ($scope.rdovalue == 'staff')
            {
                angular.forEach($scope.staff, function (user) {
                    if (!!user.selected) $scope.myArray.push(user);
                })
            }
            if ($scope.myArray.length > 0) {
                var data = {
                    "mailsubject":$scope.subject,
                    "msgtype":$scope.selecttype,
                    "msgcontent": $scope.content,
                    "bulkmailsNmobilenos": $scope.myArray
                };
                apiService.create("SendingSMSandMAILS/msgsend/", data).then(function (promise) {
                    if ($scope.selecttype == 'sms')
                    {
                        swal("SMS Sent Successfully");
                    }
                    else {
                        swal("MAIL Sent Successfully");
                    }                
                    $scope.subject = '';
                    $scope.email = '';
                    $scope.content = '';
                    $state.reload();
                });
            }
            else {
                swal("Please select atleat one record");
            }

        };

        $scope.selectedchange = function () {
            if ($scope.rdovalue == 'gen') {
                $scope.generaldiv = true;
                $scope.studiv = false;
                $scope.staffdiv = false;
                $scope.gridviewstudents = false;
                $scope.gridviewstaff = false;
            }
            else if ($scope.rdovalue == 'stu') {
                $scope.generaldiv = false;
                $scope.studiv = true;
                $scope.staffdiv = false;
                $scope.gridviewstaff = false;
             
            }
            else if ($scope.rdovalue == 'staff') {
                $scope.generaldiv = false;
                $scope.studiv = false;
                $scope.gridviewstaff = true;
             //   $scope.gridviewstudents = false;
                $scope.GetstudentorstaffDetails();
            }
        };
        $scope.selectedtype = function () {
            if ($scope.selecttype == 1) {
                $scope.mobile = true;
                $scope.mail = false;
            }
            else if ($scope.selecttype == 2) {
                $scope.mobile = false;
                $scope.mail = true;
            
            }
        };
        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.addNew = function (personalDetail) {
            $scope.personalDetails.push({
                'name': "",
                'smsmobileno': "",
                'sendmailid': "",
            });
        };
        $scope.personalDetails = [
       {
           'name': "",
           'smsmobileno': "",
           'sendmailid': "",
       } ];
        $scope.remove = function () {
            var newDataList = [];
            $scope.selectedAll = false;
            angular.forEach($scope.personalDetails, function (selected) {
                if (!selected.selected) {
                    newDataList.push(selected);
                }
            });
            $scope.personalDetails = newDataList;
        };

        //$scope.checkAll = function () {
        //    if (!$scope.selectedAll) {
        //        $scope.selectedAll = true;
        //    } else {
        //        $scope.selectedAll = false;
        //    }
        //    angular.forEach($scope.personalDetails, function (personalDetail) {
        //        personalDetail.selected = $scope.selectedAll;
        //    });
        //};
    }

})();