//By prashant latest file
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumniReportController', AlumniReportController)

    AlumniReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout']
    function AlumniReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout) {

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.showreport = true;
        $scope.showadd = false;
        $scope.sortKey = 'almst_id';
        $scope.sortReverse = true;
        $scope.IVRMMS_Id = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.searchValuetwo = "";
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.pagination = false;
        $scope.currentPage = 1;
        $scope.export_flag = true;
        $scope.Print_flag = false;
        $scope.email = true;
        $scope.ph = true;
        $scope.isRequiredmitwo = function () {
            return !$scope.newuser1.some(function (item) {
                return item.selected;
            });
        };
        $scope.allmidd = function (ac) {
            $scope.allmiddff = ac;
            var toggleStatus = $scope.allmiddff;
            angular.forEach($scope.newuser1, function (itm) {
                itm.selected = toggleStatus;
            });

        };
        $scope.printData = function () {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {

                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function (tableId) {
            $scope.export_flag = true;
            var excelname = "Alumni Report";
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = tableId;
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {

                var exportHref = Excel.tableToExcel(printSectionId_excel, 'Alumni Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }
        //TO  Get The Values in the Grid
        $scope.obj = {};
        $scope.BindData = function () {
            apiService.getDATA("AlumniSearch/Getdetails").
                then(function (promise) {
                    $scope.newuser1 = promise.yearList;
                    $scope.newuser2 = promise.classList;
                    $scope.countrylist = promise.countrylist;

                    $scope.IVRMMC_Id = 101;
                    $scope.IVRMMS_Id = 17;
                    // $scope.statelist = promise.statelistall
                    $scope.citylist1 = [];
                    $scope.citylist1 = [];
                    $scope.citylist = promise.citylist;
                    $scope.occupationlist1 = [];
                    $scope.occupationlist = promise.occupationlist;
                });
        };



        $scope.al_checkcountry = function (all, ASMCL_Id) {



            $scope.countrylistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCountry;
            angular.forEach($scope.countrylist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.countrylistarray = [];
            angular.forEach($scope.countrylist, function (qq) {
                if (qq.selected == true) {
                    $scope.countrylistarray.push({ IVRMMC_Id: qq.ivrmmC_Id })
                }
            });


            if ($scope.obj.usercheckCC == true) {
                $scope.getstate();
            }
            else {
                $scope.sectionlist = [];

            }

        }




        $scope.setfromdate = function (iddata) {

            var data = {
                "ASMAY_Id": iddata
            };

            apiService.create("ClassWiseDailyAttendance/setfromdate", data).then(function (promise) {
                if (promise !== null) {

                    $scope.newuser2 = promise.classList;

                    for (var k = 0; k < $scope.newuser1.length; k++) {

                        if ($scope.newuser1[k].asmaY_Id == iddata) {
                            var data = $scope.newuser1[k].asmaY_Year;
                        }
                    }
                    if (data != null) {
                        console.log(data);
                        var name, name1;
                        for (var i = 0; i < data.length; i++) {
                            if (i < 4) {
                                if (i == 0) {
                                    name = data[i];
                                } else {
                                    name += data[i];
                                }
                            }
                            if (i > 4) {
                                if (i == 5) {
                                    name1 = data[5];
                                } else {
                                    name1 += data[i];
                                }
                            }
                        }
                        $scope.fromDate = name;
                        $scope.toDate = name1;
                        $scope.frommon = "";
                        $scope.tomon = "";
                        $scope.fromDay = "";
                        $scope.toDay = "";

                        // For Academic From Date
                        $scope.minDatef = new Date(
                            $scope.fromDate,
                            $scope.frommon,
                            $scope.fromDay + 1);

                        $scope.maxDatef = new Date(
                            $scope.toDate,
                            $scope.tomon,
                            $scope.toDay + 365);
                    }
                }
            });
        };

        $scope.classchange = function (asmaY_Id, asmcL_Id) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.usercheckCCC;
            angular.forEach($scope.newuser2, function (role) {
                role.selected = toggleStatus;
            });

            $scope.classlistarray = [];
            angular.forEach($scope.newuser2, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })
                }
            })

        };

        $scope.ShowReport = function (asmaY_Id, asmcL_Id) {


            $scope.classlistarray = [];
            $scope.countrylistarray = [];
            $scope.statelistarray = [];

            if ($scope.obj.usercheckCCC == true) {
                $scope.classlistarray = [];
            }
            else {
                angular.forEach($scope.newuser2, function (qq) {
                    if (qq.selected == true) {
                        $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })
                    }
                });

            }


            if ($scope.obj.usercheckCountry == true) {
                $scope.countrylistarray = [];
            }
            else {

                angular.forEach($scope.countrylist, function (qq) {
                    if (qq.selected == true) {
                        $scope.countrylistarray.push({ IVRMMC_Id: qq.ivrmmC_Id })
                    }
                });
            }

            if ($scope.obj.usercheckState == true) {
                $scope.statelistarray = [];
            }
            else {


                angular.forEach($scope.statelist, function (qq) {
                    if (qq.selected == true) {
                        $scope.statelistarray.push({ IVRMMS_Id: qq.IVRMMS_Id })
                    }
                });
            }


            $scope.districtarray = [];
            if ($scope.obj.usercheckDist == true) {
                $scope.districtarray = [];
            }
            else {
                angular.forEach($scope.districtlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.districtarray.push({ IVRMMD_Id: qq.IVRMMD_Id })
                    }
                })
            }




            $scope.Occupationnew = "";
            $scope.citynew = "";
            //$scope.obj.city = parseInt($scope.obj.city)

            $scope.emailfunction = function (email) {
                if (email === true) {
                    $scope.checkemail = true;
                }
                else {
                    $scope.checkemail = false;
                }

                //  $scope.printstudents

                angular.forEach($scope.searchResult, function (dd) {
                    //  dd.selected = toggleStatus;
                    if (dd.selected == true) {
                        dd.selected = false;
                    }
                });

                angular.forEach($scope.printstudents, function (dd) {
                    //  dd.selected = toggleStatus;
                    if (dd.selected == true) {
                        dd.selected = false;
                    }
                });

                $scope.printstudents = [];
            };


            $scope.phfunction = function (ph) {
                if (ph === true) {
                    $scope.checkph = true;
                }
                else {
                    $scope.checkph = false;
                }

                angular.forEach($scope.searchResult, function (dd) {
                    //  dd.selected = toggleStatus;
                    if (dd.selected == true) {
                        dd.selected = false;
                    }
                });

                angular.forEach($scope.printstudents, function (dd) {
                    //  dd.selected = toggleStatus;
                    if (dd.selected == true) {
                        dd.selected = false;
                    }
                });
            };


            //angular.forEach($scope.citylist, function (qq) {
            //    if ($scope.obj.city.almsT_ConCity == qq.city) {
            //        $scope.citynew = qq.city;

            //    }
            //    else {
            //        $scope.citynew = "";
            //    }

            //})
            //angular.forEach($scope.occupationlist, function (qq) {
            //    if ($scope.obj.designation == qq.designation) {
            //        $scope.Occupationnew = qq.designation;
            //    }
            //    else {
            //        $scope.Occupationnew = "";
            //    }
            //})


            var city = "";
            if ($scope.obj.city != undefined && $scope.obj.city != "") {
                if ($scope.obj.city.almsT_ConCity != undefined && $scope.obj.city.almsT_ConCity != "") {
                    city = $scope.obj.city.almsT_ConCity
                }
            }



            var designation = "";
            if ($scope.obj.designation != undefined && $scope.obj.designation != "") {
                designation = $scope.obj.designation.designation
            }




            $scope.multipleBatchs = [];
            if ($scope.allmiddff == true) {
                $scope.multipleBatchs = [];
            }
            else {
                angular.forEach($scope.newuser1, function (qq) {
                    if (qq.selected == true) {
                        $scope.multipleBatchs.push({
                            ASMAY_Id: qq.asmaY_Id
                        })
                    }
                })
            }

            $scope.printstudents = [];
            $scope.searchValue = "";
            if ($scope.myform.$valid) {
                var data = {
                    // "ASMAY_Id": asmaY_Id,                   
                    "Occupation": designation,
                    "city": city,
                    "statelistarray": $scope.statelistarray,
                    "classlistnew": $scope.classlistarray,
                    "countrylistarray": $scope.countrylistarray,
                    "districtlistarray": $scope.districtarray,
                    "multipleBatchs": $scope.multipleBatchs



                };

                apiService.create("AlumniSearch/Getdetailsreport", data).
                    then(function (promise) {
                        $scope.searchResult = [];
                        $scope.searchResult2 = [];
                        if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {


                            $scope.searchResult2 = promise.searchstudentDetails;

                            if (promise.searchstudentDetails.length > 0) {
                                $scope.searchResult = $scope.searchResult2;
                            }
                        }



                        //if (promise.searchstudentDetails.length > 0) {
                        //    $scope.searchResult = promise.searchstudentDetails;
                        //    angular.forEach($scope.searchResult, function (objectt) {
                        //        if (objectt.AMST_PerStreet != null && objectt.AMST_PerStreet!="" && objectt.AMST_PerStreet.length > 0) {
                        //            var string = objectt.AMST_PerStreet;
                        //            if (string[string.length - 1] == ',') {
                        //                var n = string.lastIndexOf(",");
                        //                objectt.AMST_PerStreet = string.substring(0, n);
                        //            }     
                        //            var so = string.substr(0, 1);
                        //            if (so == ',') {                                        
                        //                objectt.AMST_PerStreet = string.substring(1);
                        //            }
                        //        }
                        //        if (objectt.AMST_PerArea != null && objectt.AMST_PerArea != "" && objectt.AMST_PerArea.length > 0) {
                        //            var string1 = objectt.AMST_PerArea;
                        //            if (string1[string1.length - 1] == ',') {
                        //                var n1 = string1.lastIndexOf(",");
                        //                objectt.AMST_PerArea = string1.substring(0, n1);
                        //            }
                        //            var so1 = string1.substr(0, 1);
                        //            if (so1 == ',') {
                        //                objectt.AMST_PerArea = string1.substring(1);
                        //            }
                        //        }
                        //    });
                        //    $scope.presentCountgrid = $scope.searchResult.length;
                        //}
                        else {
                            $scope.searchResult = {};
                            swal("Records Not Found");
                        }
                    });
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.getstate = function (qq) {

            $scope.countrylistarray = [];

            angular.forEach($scope.countrylist, function (aa) {
                if (aa.selected == true) {
                    $scope.countrylistarray.push({ IVRMMC_Id: aa.ivrmmC_Id })
                }

            });
            var data = {
                //"IVRMMC_Id": qq.ivrmmC_Id,
                "countrylistarray": $scope.countrylistarray

            }
            apiService.create("AlumniSearch/getstate", data).then(function (promise) {
                if (promise.statelist.length > 0 || promise.statelist != null) {
                    $scope.statelist = promise.statelist

                    if (promise.citylist.length > 0 || promise.citylist != '' || promise.citylist != null) {
                        var cnt = 0;
                        $scope.citylist = [];
                        angular.forEach(promise.citylist, function (qq) {
                            cnt = cnt + 1;
                            $scope.citylist.push({ id: cnt, city: qq.almsT_ConCity })
                        })
                    }

                    if (promise.occupationlist.length > 0 || promise.occupationlist != '' || promise.occupationlist != null) {
                        var cnt = 0;
                        angular.forEach(promise.occupationlist, function (qq) {
                            cnt = cnt + 1;
                            $scope.occupationlist1.push({ id: cnt, designation: qq.designation })
                        })
                    }

                    $scope.citylist = $scope.citylist1;
                    $scope.occupationlist = $scope.occupationlist1;
                }
                else {
                    swal('No Data Found!!!')
                }

            })
        }
        $scope.all_checkC = function (all, ASMCL_Id) {
            $scope.statelistarray = [];
            $scope.obj.usercheckC = all;
            var toggleStatus = $scope.obj.usercheckState;
            angular.forEach($scope.statelist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.statelistarray = [];
            angular.forEach($scope.statelist, function (qq) {
                if (qq.selected == true) {
                    $scope.statelistarray.push({ IVRMMS_Id: qq.ivrmmS_Id })
                }
            });
            $scope.getdistrict();


        };

        $scope.isOptionsRequiredcountry = function () {
            return !$scope.countrylist.some(function (item) {
                return item.selected;
            });
        };



        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.getdistrict = function (IVRMMC_Id, qq) {


            var state_id = 0;
            if ($scope.IVRMMS_Id.ivrmmS_Id != null && $scope.IVRMMS_Id.ivrmmS_Id != 0) {
                state_id = $scope.IVRMMS_Id.ivrmmS_Id
            }
            var countryid = 0

            if ($scope.IVRMMC_Id.ivrmmC_Id != null && $scope.IVRMMC_Id.ivrmmC_Id != 0) {
                countryid = $scope.IVRMMC_Id.ivrmmC_Id
            }

            $scope.statelistarray = [];

            angular.forEach($scope.statelist, function (aa) {
                if (aa.selected == true) {
                    $scope.statelistarray.push({ IVRMMS_Id: aa.IVRMMS_Id })
                }

            });


            var data = {
                //"IVRMMS_Id": state_id,
                //"IVRMMC_Id": countryid,
                "statelistarray": $scope.statelistarray

            }
            apiService.create("AlumniSearch/getdistrict", data).then(function (promise) {
                if (promise != null) {
                    if (promise.districtlist.length > 0 || promise.districtlist != null) {
                        $scope.districtlist = promise.districtlist
                    }
                    //if (promise.citylist.length > 0 || promise.citylist != null) {
                    //    $scope.citylist = promise.citylist
                    //}
                }


                else {
                    swal('No Data Found!!!')
                }

            })
        }



        $scope.al_checkclass = function (all, ASMCL_Id) {



            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCCC;
            angular.forEach($scope.newuser2, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.newuser2, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })
                }
            });


            if ($scope.obj.usercheckCC == true) {

                $scope.classflag = true;
            }


        }


        $scope.togchkbxS = function () {
            $scope.districtarray = [];
            angular.forEach($scope.districtlist, function (qq) {
                if (qq.selected == true) {
                    $scope.districtarray.push({ IVRMMD_Id: qq.IVRMMD_Id })
                }
            })
        }


        $scope.all_checkdist = function (all) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.obj.usercheckDist;
            angular.forEach($scope.districtlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.districtarray = [];
            angular.forEach($scope.districtlist, function (qq) {
                if (qq.selected == true) {
                    $scope.districtarray.push({ IVRMMD_Id: qq.IVRMMD_Id })
                }
            })



        };



        $scope.isOptionsRequireddis = function () {
            return !$scope.studentlist.some(function (item) {
                return item.selected;
            });
        };





        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;


            if ($scope.searchResult != null && $scope.searchResult.length > 0) {

                angular.forEach($scope.searchResult, function (t3) {
                    t3.selected = toggleStatus;
                    if (t3.selected == true) {
                        var al_cnt = 0;
                        angular.forEach($scope.printstudents, function (rt) {
                            //if (rt.almst_id == t3.almst_id) {
                            //if ((rt.almst_id == t3.almst_id && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.amsT_FirstName == t3.amsT_FirstName && rt.ALMST_FullAddess == t3.ALMST_FullAddess)) {
                            //    al_cnt += 1;
                            //}

                            if (((rt.almst_id == t3.almst_id && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.amsT_FirstName == t3.amsT_FirstName && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.almst_id == t3.almst_id && rt.amsT_FirstName == t3.amsT_FirstName)) && (rt.ASMAY_Id_Left != t3.ASMAY_Id_Left)) {
                                al_cnt += 1;
                            }
                            else if ((rt.amsT_FirstName == t3.amsT_FirstName) && (rt.ASMAY_Id_Left != t3.ASMAY_Id_Left) && (rt.ASMAY_Id_Join != t3.ASMAY_Id_Join)) {
                                al_cnt += 1;
                            }
                            else if (rt.almst_id == t3.almst_id) {
                                al_cnt += 1;
                            }
                        });
                        if (al_cnt === 0) {

                            // $scope.printstudents.push(t3);

                            if ($scope.email == true || $scope.ph == true) {
                                if ($scope.email == true && $scope.ph == true) {
                                    $scope.printstudents.push({
                                        amsT_DOB: t3.amsT_DOB,
                                        city: t3.city,
                                        amsT_BloodGroup: t3.amsT_BloodGroup,
                                        IVRMMD_Name: t3.IVRMMD_Name,
                                        ALMST_FatherName: t3.ALMST_FatherName,
                                        ASMCL_Id_Join: t3.ASMCL_Id_Join,
                                        Leftclass: t3.Leftclass,
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_emailId: t3.amsT_emailId, amsT_MobileNo: t3.amsT_MobileNo, ALMST_PerPincode: t3.ALMST_PerPincode
                                    });
                                }
                                else if ($scope.email == true && $scope.ph == false) {
                                    $scope.printstudents.push({
                                        amsT_DOB: t3.amsT_DOB,
                                        city: t3.city,
                                        amsT_BloodGroup: t3.amsT_BloodGroup,
                                        IVRMMD_Name: t3.IVRMMD_Name,
                                        ALMST_FatherName: t3.ALMST_FatherName,
                                        ASMCL_Id_Join: t3.ASMCL_Id_Join,
                                        Leftclass: t3.Leftclass,
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_emailId: t3.amsT_emailId, ALMST_PerPincode: t3.ALMST_PerPincode
                                    });
                                }
                                else if ($scope.email == false && $scope.ph == true) {
                                    $scope.printstudents.push({
                                        amsT_DOB: t3.amsT_DOB,
                                        city: t3.city,
                                        amsT_BloodGroup: t3.amsT_BloodGroup,
                                        IVRMMD_Name: t3.IVRMMD_Name,
                                        ALMST_FatherName: t3.ALMST_FatherName,
                                        ASMCL_Id_Join: t3.ASMCL_Id_Join,
                                        Leftclass: t3.Leftclass,
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_MobileNo: t3.amsT_MobileNo, ALMST_PerPincode: t3.ALMST_PerPincode
                                    });
                                }
                            }
                            else {
                                if ($scope.email == false && $scope.ph == false) {
                                    $scope.printstudents.push({
                                        amsT_DOB: t3.amsT_DOB,
                                        city: t3.city,
                                        amsT_BloodGroup: t3.amsT_BloodGroup,
                                        IVRMMD_Name: t3.IVRMMD_Name,
                                        ALMST_FatherName: t3.ALMST_FatherName,
                                        ASMCL_Id_Join: t3.ASMCL_Id_Join,
                                        Leftclass: t3.Leftclass,
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, ALMST_PerPincode: t3.ALMST_PerPincode
                                    });
                                }
                            }

                        }
                    }
                });

            }
            //angular.forEach($scope.searchResult, function (itm) {
            //    itm.selected = toggleStatus;
            //    if (itm.ALMST_FullAddess !== null && itm.ALMST_FullAddess !== "") {
            //        if ($scope.all == true) {

            //            if ($scope.printstudents.indexOf(itm) === -1) {
            //                $scope.printstudents.push(itm);
            //                $scope.export_flag = false;
            //            }
            //        }


            //        else {
            //            $scope.printstudents.splice(itm);
            //        }
            //    }
            //});


            //added

        }


        //old code

        //$scope.toggleAll = function () {
        //    $scope.printstudents = [];
        //    var toggleStatus = $scope.all;


        //    if ($scope.searchResult != null && $scope.searchResult.length > 0) {

        //        angular.forEach($scope.searchResult, function (t3) {
        //            t3.selected = toggleStatus;
        //            if (t3.selected == true) {
        //                var al_cnt = 0;
        //                angular.forEach($scope.printstudents, function (rt) {
        //                    if ((rt.almst_id == t3.almst_id && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.amsT_FirstName == t3.amsT_FirstName && rt.ALMST_FullAddess == t3.ALMST_FullAddess)) {
        //                        al_cnt += 1;
        //                    }
        //                });
        //                if (al_cnt === 0) {

        //                    $scope.printstudents.push(t3);

        //                }
        //            }
        //        });


        //    }

        //}


        //$scope.sortableOptions = {
        //    stop: function (e, ui) {
        //        for (var index in $scope.printstudents) {
        //            $scope.printstudents[index].amsT_FirstName = Number(index) + 1;

        //        }
        //    }
        //};
        //$scope.exportToExcel = function (tableId) {
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {

        //        var exportHref = Excel.tableToExcel(tableexcell, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //        //$state.reload();
        //    }
        //    else {
        //        swal("Please select records to be exported");
        //    }

        //}

        //$scope.exportToExcel = function () {           
        //    var printSectionId = '#tableexcellnew';
        //    //if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(printSectionId, 'Alumni Report');
        //        $timeout(function () {
        //            var a = document.createElement('a');
        //            a.href = exportHref;
        //            a.download = "Alumni Report.xls";
        //            document.body.appendChild(a);
        //            a.click();
        //            a.remove();
        //        }, 100);
        //    //}
        //    //else {
        //    //    swal("Please Select Records to be Exported");
        //    //}
        //};


        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.searchResult.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                if ($scope.printstudents.length == 0) {
                    //  $scope.printstudents.push(SelectedStudentRecord);



                    if ($scope.email == true || $scope.ph == true) {
                        if ($scope.email == true && $scope.ph == true) {
                            $scope.printstudents.push({
                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                Leftclass: SelectedStudentRecord.Leftclass,
                                almst_id: SelectedStudentRecord.almst_id,
                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                            });
                        }
                        else if ($scope.email == true && $scope.ph == false) {
                            $scope.printstudents.push({
                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                Leftclass: SelectedStudentRecord.Leftclass,
                                almst_id: SelectedStudentRecord.almst_id,
                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                            });
                        }
                        else if ($scope.email == false && $scope.ph == true) {
                            $scope.printstudents.push({
                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                Leftclass: SelectedStudentRecord.Leftclass,
                                almst_id: SelectedStudentRecord.almst_id,
                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                            });
                        }
                    }


                    else if ($scope.email == false || $scope.ph == false) {
                        if ($scope.email == false && $scope.ph == false) {
                            $scope.printstudents.push({
                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                Leftclass: SelectedStudentRecord.Leftclass,
                                almst_id: SelectedStudentRecord.almst_id,
                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                            });
                        }
                        else if ($scope.email == true && $scope.ph == false) {
                            $scope.printstudents.push({
                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                Leftclass: SelectedStudentRecord.Leftclass,
                                almst_id: SelectedStudentRecord.almst_id,
                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                            });
                        }
                        else if ($scope.email == false && $scope.ph == true) {
                            $scope.printstudents.push({
                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                Leftclass: SelectedStudentRecord.Leftclass,
                                almst_id: SelectedStudentRecord.almst_id,
                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                            });
                        }
                    }

                }
                else {



                    if ($scope.searchResult != null && $scope.searchResult.length > 0) {

                        angular.forEach($scope.searchResult, function (t3) {
                            if (t3.selected == true) {
                                var al_cnt = 0;
                                angular.forEach($scope.printstudents, function (rt) {
                                    if (((rt.almst_id == t3.almst_id && rt.ALMST_FullAddess == t3.ALMST_FullAddess)
                                        || (rt.amsT_FirstName == t3.amsT_FirstName && rt.ALMST_FullAddess == t3.ALMST_FullAddess)
                                        || (rt.almst_id == t3.almst_id && rt.amsT_FirstName == t3.amsT_FirstName)) && (rt.ASMAY_Id_Left != t3.ASMAY_Id_Left)) {
                                        al_cnt += 1;
                                    }
                                    else if ((rt.amsT_FirstName == t3.amsT_FirstName) && (rt.ASMAY_Id_Left != t3.ASMAY_Id_Left) && (rt.ASMAY_Id_Join != t3.ASMAY_Id_Join)) {
                                        al_cnt += 1;
                                    }
                                    else if (rt.almst_id == t3.almst_id) {
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {

                                    // $scope.printstudents.push(SelectedStudentRecord);


                                    if ($scope.email == true || $scope.ph == true) {
                                        if ($scope.email == true && $scope.ph == true) {
                                            $scope.printstudents.push({
                                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                                Leftclass: SelectedStudentRecord.Leftclass,
                                                almst_id: SelectedStudentRecord.almst_id,
                                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo
                                            });
                                        }
                                        else if ($scope.email == true && $scope.ph == false) {
                                            $scope.printstudents.push({
                                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                                Leftclass: SelectedStudentRecord.Leftclass,
                                                almst_id: SelectedStudentRecord.almst_id,
                                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId
                                            });
                                        }
                                        else if ($scope.email == false && $scope.ph == true) {
                                            $scope.printstudents.push({
                                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                                Leftclass: SelectedStudentRecord.Leftclass,
                                                almst_id: SelectedStudentRecord.almst_id,
                                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo
                                            });
                                        }
                                    }
                                    else if ($scope.email == false || $scope.ph == false) {
                                        if ($scope.email == false && $scope.ph == false) {
                                            $scope.printstudents.push({
                                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                                Leftclass: SelectedStudentRecord.Leftclass,
                                                almst_id: SelectedStudentRecord.almst_id,
                                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess
                                            });
                                        }
                                        else if ($scope.email == true && $scope.ph == false) {
                                            $scope.printstudents.push({
                                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                                Leftclass: SelectedStudentRecord.Leftclass,
                                                almst_id: SelectedStudentRecord.almst_id,
                                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId
                                            });
                                        }
                                        else if ($scope.email == false && $scope.ph == true) {
                                            $scope.printstudents.push({
                                                ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                                Leftclass: SelectedStudentRecord.Leftclass,
                                                almst_id: SelectedStudentRecord.almst_id,
                                                ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                                ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                                ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                                ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo
                                            });
                                        }
                                    }


                                }
                            }
                        });

                    }
                }



                $scope.export_flag = false;
            }
            else {

                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }


        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });

        $scope.searchValue = '';
        $scope.filterValue = function () {
            return ($scope.AMST_FirstName + ' ' + $scope.AMST_MiddleName + ' ' + $scope.Amst_LastName).indexOf($scope.searchValue) >= 0 ||
                ($scope.AMST_AdmNo).indexOf($scope.searchValue) >= 0 ||
                ($scope.AMST_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                ($scope.asmcl_classname).indexOf($scope.searchValue) >= 0
                || ($scope.asmc_sectionname).indexOf($scope.searchValue) >= 0 ||
                ($scope.classes).indexOf($scope.searchValue) >= 0;
        };

        $scope.presentCountgrid = 0; $scope.AbsentCountgrid = 0;



        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.submitted = false;

        $scope.propertyName = 'AMST_FirstName';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };


        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };
        $scope.radio_but_switch = function () {
            if ($scope.sall == 1) {
                $scope.showreport = true;
                $scope.showadd = false;
            }
            else {
                $scope.showreport = false;
                $scope.showadd = true;
            }
        }

        $scope.printDataadd = function () {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById("SRKVSStudentAddressBook").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Select atleast one record !")
            }
        }




        $scope.clear = function () {

            $state.reload();
            $scope.asmaY_Id = "";
            $scope.FromDate = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.obj.designation = "";
            $scope.IVRMMC_ID = "";
            //$scope.ivrmmS_Id = "";
            //$scope.$parent.IVRMMD_Id = "";
            $scope.obj.city = "";

            $scope.reportdetails = "";
            $scope.submitted = false;
            $scope.searchResult = {};
            $scope.IsHiddendown = false;
            $scope.export_flag = true;

            $scope.email = false;
            $scope.ph = false;

            $scope.myform.$setPristine();
            $scope.myform.$setUntouched();
        };




    }

})();