
(function () {
    'use strict';
    angular
.module('app')
.controller('SportStudentParticipationReportController', SportStudentParticipationReportController)

    SportStudentParticipationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportStudentParticipationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http,Excel, $timeout, $filter, superCache) {




        $scope.searchValue = "";
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.screport = false;


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        //============TO  GEt The Values iN Grid

        $scope.BindData = function () {

            apiService.getDATA("SportStudentParticipationReport/Getdetails").
                then(function (promise) {
                    debugger;

                    $scope.yearlt = promise.yearlist;

                })
        };


        //=================================Get Class
        $scope.get_class = function () {
            debugger;
            $scope.usercheck23 = false;
            $scope.usercheck = false;
            $scope.Cumureport = false;
            $scope.export = false;
            $scope.screport = false;
            //$scope.ASMAY_Id = "";
            angular.forEach($scope.houseList, function (hs) {
                hs.select = false;
            });
            angular.forEach($scope.sectionDropdown, function (hs) {
                hs.select = false;
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("SportStudentParticipationReport/get_class", data)
                .then(function (promise) {
                    $scope.classList = promise.classList;
                    $scope.houseList = promise.houseList;

                })
        }

        //=================================Get Section
        $scope.get_section = function () {

            $scope.usercheck23 = false;

            $scope.Cumureport = false;
            $scope.export = false;
            $scope.screport = false;

            angular.forEach($scope.sectionDropdown, function (hs) {
                hs.select = false;
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportStudentParticipationReport/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;

                })
        }

        //$scope.clscatId = 0;
        $scope.columnSort = false;
        $scope.isOptionsRequired = function () {
            return !$scope.stuDropdown.some(function (options) {
                return options.Selected;
            });
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        // TO Show The Data
        $scope.submitted = false;
        $scope.showdetails = function () {
            $scope.selectedhouselist = [];
            $scope.selectedSectionlist = [];
            debugger;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {


                angular.forEach($scope.houseList, function (hous) {
                    if (hous.select == true) {
                        $scope.selectedhouselist.push({ spccmH_Id: hous.spccmH_Id });
                    }
                });

                if ($scope.Type == 'House') {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": 0,
                        //"ASMS_Id": 0,
                        "Type": $scope.Type,
                        //"SPCCMH_Id": $scope.SPCCMH_Id,

                        selectedhouselist: $scope.selectedhouselist,
                    }

                }
                else if ($scope.Type == 'CS') {

                    angular.forEach($scope.sectionDropdown, function (section) {
                        if (section.select == true) {
                            $scope.selectedSectionlist.push({ asmS_Id: section.asmS_Id });
                        }
                    });
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        //"ASMS_Id": $scope.ASMS_Id,
                        "Type": $scope.Type,
                        //"SPCCMH_Id": $scope.SPCCMH_Id,
                        selectedhouselist: $scope.selectedhouselist,
                        selectedSectionlist: $scope.selectedSectionlist,
                    }
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SportStudentParticipationReport/showdetails", data).
                    then(function (promise) {
                        $scope.newuser = promise.viewlist;
                        if ($scope.newuser.length > 0) {
                            $scope.newuser = promise.viewlist;

                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;

                            angular.forEach($scope.yearlt, function (fff) {
                                if (fff.asmaY_Id == $scope.ASMAY_Id) {
                                    $scope.yearname = fff.asmaY_Year;
                                }
                            })

                        }
                        else {
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            swal("From this House Students are not Participating!");

                        }
                    })
            }
        };

        $scope.cancel = function () {
            //$scope.ASMAY_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMS_Id = "";
            //$scope.SPCCME_Id = "";
            //$scope.SPCCMH_Id = "";

            //$scope.Cumureport = false;
            //$scope.screport = false;
            //$scope.export = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();

            $state.reload();
        }

        //for print
        $scope.Print = function () {

            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
        }

        // end for print

        $scope.exportToExcel = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }



        //===========================================Radio

        $scope.changeRadiobtn = function () {
            $scope.screport = false;
            $scope.export = false;
            $scope.Cumureport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            debugger;
            if ($scope.Type == 'CS') {
                $scope.usercheck23 = false;
                $scope.usercheck = false;

                $scope.ASMAY_Id = "";
                angular.forEach($scope.houseList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionDropdown, function (hs) {
                    hs.select = false;
                });
                $scope.BindData();
            }
            else if ($scope.Type == 'House') {
                $scope.usercheck23 = false;
                $scope.usercheck = false;

                $scope.ASMAY_Id = "";
                angular.forEach($scope.houseList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionDropdown, function (hs) {
                    hs.select = false;
                });
                $scope.BindData();
            }
        }

        //////////=========================================================
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.houseList, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.houseList.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.houseList.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.spccmH_HouseName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionDropdown, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionDropdown.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired23 = function () {
            return !$scope.sectionDropdown.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }




      ////  $scope.DeleteRecord = {};
      ////  $scope.EditRecord = {};
      ////  $scope.obj = {};
      ////  $scope.studentlist = false;
      ////  $scope.currentPage = 1;
      ////  $scope.itemsPerPage = 5;

      ////  $scope.ddate = new Date();
      ////  var paginationformasters;
      ////  var copty;
      ////  var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
      ////  if (ivrmcofigsettings.length > 0) {
      ////      paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
      ////      copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
      ////  }
      ////  $scope.coptyright = copty;
      ////  $scope.sortKey = "regno";   //set the sortKey to the param passed
      ////  $scope.reverse = true; //if true make it false and vice versa

      ////  $scope.presentCountgrid = 0;

      ////  $scope.usrname = localStorage.getItem('username');
      ////  var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
      ////  if (admfigsettings.length > 0) {
      ////      var logopath = admfigsettings[0].asC_Logo_Path;
      ////  }

      ////  $scope.imgname = logopath;


      ////  //TO  GEt The Values iN Grid

      ////  $scope.BindData = function () {

      ////      apiService.getDATA("SportStudentParticipationReport/Getdetails").
      //// then(function (promise) {

      ////     $scope.yearlt = promise.yearlist;
      ////     $scope.eventname = promise.eventname;
      ////     $scope.asmcL_Id = "";
      ////     $scope.asmS_Id = "";
      ////     $scope.classDropdown = "";
      ////     $scope.sectionDropdown = "";


      //// })
      ////  };

      ////  $scope.stdrecord = true;
      ////  $scope.onselectradio = function () {
      ////      var obj = {
      ////          "radiotype": $scope.qualification_type
      ////      }
      ////      apiService.create("SportStudentParticipationReport/getevent/", obj).then(function (promise) {
      ////          $scope.eventname = promise.eventname;
      ////      })

      ////      if ($scope.qualification_type == 'all') {
      ////          $scope.stuDropdown = "";
      ////          $scope.stdrecord = false;
      ////      }
      ////      else {
      ////          $scope.stdrecord = true;
      ////      }
      ////  };

      ////  $scope.qualification_type = 'others';

      ////  $scope.get_class = function () {
             

      ////      $scope.asmS_Id = "";
      ////      var data = {
      ////          "ASMAY_Id": $scope.asmaY_Id
      ////      }
      ////      apiService.create("SportStudentParticipationReport/get_class", data)
      ////          .then(function (promise) {
      ////              $scope.classDropdown = promise.classList;

      ////          })
      ////  }
      ////  $scope.get_section = function () {
      ////      $scope.stuDropdown = "";
      ////      $scope.ASMS_Id = "";

      ////      var data = {
      ////          "ASMAY_Id": $scope.asmaY_Id,
      ////          "ASMCL_Id": $scope.ASMCL_Id
      ////      }
      ////      apiService.create("SportStudentParticipationReport/get_section", data)
      ////          .then(function (promise) {
      ////              $scope.sectionDropdown = promise.sectionList;


      ////          })
      ////  }
      ////  $scope.get_student = function () {
      ////      $scope.stuDropdown = "";

      ////      var data = {
      ////          "ASMAY_Id": $scope.asmaY_Id,
      ////          "ASMS_Id": $scope.ASMS_Id,
      ////          "ASMCL_Id": $scope.ASMCL_Id
      ////      }
      ////      apiService.create("SportStudentParticipationReport/get_student", data)
      ////          .then(function (promise) {
      ////              if (promise.studentList1.length > 0) {
      ////                  $scope.stdrecord = true;
      ////                  $scope.stuDropdown = promise.studentList1;
      ////              }
                   


      ////          })

      ////  }
      ////  //$scope.clscatId = 0;
      ////  $scope.columnSort = false;
       
      ////  $scope.isOptionsRequired = function () {
      ////      return !$scope.stuDropdown.some(function (options) {
      ////          return options.stud;
      ////      });
      ////  }

      ////  $scope.sort = function (keyname) {
      ////      $scope.sortKey = keyname;   //set the sortKey to the param passed
      ////      $scope.reverse = !$scope.reverse; //if true make it false and vice versa
      ////  }

      ////  $scope.interacted = function (field) {
      ////      return $scope.submitted;
      ////  };



      ////  // TO Show The Data
      ////  $scope.submitted = false;
       
      ////  $scope.showdetails = function () {
             
      ////      $scope.submitted = true;
      ////      if ($scope.myForm.$valid) {
      ////          $scope.Cumureport = false;

      ////          if ($scope.stuDropdown != "" && $scope.stuDropdown != null) {
      ////              if ($scope.stuDropdown.length > 0) {
      ////                  $scope.selectedStdList = [];
      ////                  for (var i = 0; i < $scope.stuDropdown.length; i++) {
      ////                      if ($scope.stuDropdown[i].stud == true) {
      ////                          $scope.selectedStdList.push($scope.stuDropdown[i]);
      ////                      }
      ////                  }
      ////              }
      ////          }
      ////          var data = {

      ////              "ASMAY_Id": $scope.asmaY_Id,
      ////              "ASMCL_Id": $scope.ASMCL_Id,
      ////              "ASMS_Id": $scope.ASMS_Id,
      ////              "SPCCME_Id": $scope.SPCCME_Id,
      ////              "StudentList": $scope.selectedStdList,
      ////              "radiotype": $scope.qualification_type
      ////          }
      ////          var config = {
      ////              headers: {
      ////                  'Content-Type': 'application/json;'
      ////              }
      ////          }                
      ////          apiService.create("SportStudentParticipationReport/showdetails", data).
      ////                   then(function (promise) {
                              
      ////                       if (promise.viewlist.length > 0) {
      ////                               $scope.newuser = promise.viewlist;
      ////                               $scope.presentCountgrid = $scope.newuser.length;
      ////                               $scope.Cumureport = true;
      ////                               $scope.screport = true;
      ////                               $scope.export = true;
      ////                              }
      ////                           else {
      ////                           swal("No Records Found");
      ////                           $scope.cancel();
      ////                           }
                           
      ////                   })
      ////      }
      ////  };

      ////  $scope.cancel = function () {
      ////      $scope.asmaY_Id = "";
      ////      $scope.ASMCL_Id = "";
      ////      $scope.ASMS_Id = "";
      ////      $scope.SPCCME_Id = "";
      ////      $scope.stdrecord = false;
      ////      $scope.stuDropdown = "";
      ////      $scope.Cumureport = false;
      ////      $scope.screport = false;
      ////      $scope.export = false;
      ////      $scope.submitted = false;
      ////      $scope.myForm.$setPristine();
      ////      $scope.myForm.$setUntouched();
      ////  }

      ////  //for print
      ////  $scope.Print = function () {

      ////      if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
      ////          var innerContents = document.getElementById("printSectionId").innerHTML;
      ////          var popupWinindow = window.open('');
      ////          popupWinindow.document.open();
      ////          popupWinindow.document.write('<html><head>' +
      ////    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
      ////'<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
      //// '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
      ////'</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
      ////    );
      ////          popupWinindow.document.close();
      ////      }
      ////  }

      ////  // end for print

      ////  $scope.exportToExcel = function (table) {
             
      ////      //if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
      ////          var exportHref = Excel.tableToExcel(table, 'sheet name');
      ////          $timeout(function () { location.href = exportHref; }, 100);
      ////     // }
      ////     // else {
      ////       //   swal("Please Select Records to be Exported");
      ////     // }
      ////  }

      ////  $scope.searchchkbx = "";
      ////  $scope.filterchkbx = function (obj) {
      ////      return (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
      ////  }
      ////  $scope.togchkbx = function () {
      ////      $scope.usercheck = $scope.stuDropdown.every(function (options) {
      ////          return options.stud;
      ////      });
      ////  }
      ////  $scope.all_check = function () {
      ////      var checkStatus = $scope.usercheck;
      ////      angular.forEach($scope.stuDropdown, function (itm) {
      ////          itm.stud = checkStatus;
      ////      });
      ////  }

    }

})();