(function () {
    'use strict';

    angular
        .module('app')
        .controller('EmailSMSCount', EmailSMSCount);

    EmailSMSCount.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', 'Excel', '$timeout'];

    function EmailSMSCount($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache,Excel, $timeout) {

        $scope.obj = {};
        $scope.table_flag = false;
        $scope.setCheked = function () {
            $scope.obj.check1 = true;
            $scope.obj.check2 = true;
        }
       
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
                $scope.imgname = logopath;
            }

        }
      
        $scope.checkedStatus = function () {
            $scope.sms_mail_count = "";
            if ($scope.obj.check1 == true) {
                $scope.obj.check1 = true;
            }
            else {
                $scope.obj.check1 = false;
            }
            if ($scope.obj.check2 == true) {
                $scope.obj.check2 = true;
            }
            else {
                $scope.obj.check2 = false;
            }
        }
        $scope.rdochange = function () {
            $scope.sms_mail_count = "";
        }

        $scope.submitted = false;
        $scope.validatetodate = function (data1) {
            $scope.sms_mail_count = "";
            $scope.toDate = data1;
            $scope.minDatet = new Date(
           $scope.toDate.getFullYear(),
           $scope.toDate.getMonth(),
          $scope.toDate.getDate());

            $scope.maxDatet = new Date(
          $scope.toDate = "",
          $scope.toDate.getMonth(),
         $scope.toDate.getDate());
        }


        $scope.getReport = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            if ($scope.myForm.$valid) {
                var data = {
                    "rdbbutton": $scope.radioption,
                    "staffChecked": $scope.obj.check2,
                    "studChecked": $scope.obj.check1,
                    "start_date": new Date($scope.Start_Date).toDateString(),
                    "end_date": new Date($scope.End_Date).toDateString(),
                }

                apiService.create("BirthDay/getEmailSMSCount/", data).
                    then(function (promise) {
                        debugger;
               if ($scope.radioption == "smscount") {
                   if (promise.count > 0) {
                       $scope.sms_mail_count = promise.sms_mail_count;
                       $scope.presentCountgrid = $scope.sms_mail_count.length;
                   }
                   else {
                       swal("No Records Found");
                       $scope.sms_mail_count = "";
                   }
                 
               }
               else if ($scope.radioption == "emailcount") {
                   if (promise.count > 0) {
                       $scope.sms_mail_count = promise.mail_count_list;
                       $scope.presentCountgrid = $scope.sms_mail_count.length;
                   }
                   else {
                       swal("No Records Found");
                       $scope.sms_mail_count = "";
                   }
                   
               }

           });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchByColumn = function (search, searchColumn) {
            
            if (search != "" && search != null && search != undefined) {
               
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn,
                    "rdbbutton": $scope.radioption,
                    "start_date": new Date($scope.Start_Date).toDateString(),
                    "end_date": new Date($scope.End_Date).toDateString(),
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BirthDay/SearchByColumn", data)

          .then(function (promise) {

              if (promise.count == 0) {
                  swal("No Records Found");
                  $state.reload();
              }
              if (promise.message != "" && promise.message != null) {
                  swal(promise.message);
                  if ($scope.radioption == "smscount") {
                     
                      if (promise.sms_mail_count != null && promise.sms_mail_count != "") {
                          $scope.sms_mail_count = promise.sms_mail_count;
                          $scope.presentCountgrid = $scope.sms_mail_count.length;
                      }
                     
                  }
                  else if ($scope.radioption == "emailcount") {
                     
                      if (promise.mail_count_list != null && promise.mail_count_list != "") {
                          $scope.sms_mail_count = promise.mail_count_list;
                          $scope.presentCountgrid = $scope.sms_mail_count.length;
                      }
                  }
              }
              else {
                  $scope.search = "";
                  if ($scope.radioption == "smscount") {
                     
                      if (promise.sms_mail_count != null && promise.sms_mail_count != "") {
                          $scope.sms_mail_count = promise.sms_mail_count;
                          $scope.presentCountgrid = $scope.sms_mail_count.length;
                      }

                  }
                  else if ($scope.radioption == "emailcount") {
                     
                      if (promise.mail_count_list != null && promise.mail_count_list != "") {
                          $scope.sms_mail_count = promise.mail_count_list;
                          $scope.presentCountgrid = $scope.sms_mail_count.length;
                      }
                  }
                 
              }

          })
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        }
        $scope.getcolumnId = function (ColumnId) {
            if (ColumnId == "3") {
                swal("Enter date in dd/MM/yyyy format");
            }
        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.printdatatable = [];
        $scope.toggleAll = function () {

                var toggleStatus = $scope.details;
                angular.forEach($scope.sms_mail_count, function (itm) {
                    itm.Selected = toggleStatus;
                    if ($scope.details == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
          
            $scope.details = $scope.sms_mail_count.every(function (itm)
                { return itm.Selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
        }
        $scope.Print = function () {
          
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionId").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
           '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
          '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
              );
                    popupWinindow.document.close();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
        }

        $scope.ExportToExcel = function (tableId) {
            
           
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
