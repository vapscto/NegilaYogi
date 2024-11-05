(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumniHomepageController1', AlumniHomepageController1)

    AlumniHomepageController1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function AlumniHomepageController1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        $scope.userPrivileges = "";
        $scope.search = "";
        var pageid = $stateParams.pageId;
        $scope.ddate = {};
        $scope.obj = {};
        
        $scope.ddate = new Date();
        $scope.editalumni = false;
      //  $scope.editflga = false;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings!=null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.currentPage = 1;
        $scope.itemsPerPages = 10;
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //Loading the Intial Data Function
        $scope.disablefag = false;
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            //promise.flag
          
            apiService.getURI("AlumniMembership/get_intial_data", pageid).then(function (promise) {
                $scope.pcity = false;
                $scope.alumniedit = false;
                $scope.alumnilist = promise.alumnilist;
                $scope.alumnilistnew = promise.alumnilistnew;
                $scope.membercategory = promise.membercategory;
                if (promise.flag == "true") {
                    $scope.disablefag = true;
                }
                if (promise.alumnitrue == false) {
                    $scope.adminslumni = true;
                    $scope.almstudentDetails = promise.almdetails;
                    $scope.yearlst = promise.fillyear;
                    $scope.readmitted_yearlst = promise.fillyear;


                    $scope.classlist = promise.fillclass;

                    $scope.readmitted_classlist = promise.fillclass;
                    $scope.arrlist = promise.countryDrpDown;
                    $scope.arrStatelist2 = promise.statedropdown;
                    $scope.arrStatelist3 = promise.statedropdown;
                    $scope.arrDistrcitlist2 = promise.districtdropdown;
                    
                }
                else {
                    $scope.alumniedit = true;
                    $scope.adminslumni = false;
                    $scope.studentquali = [];
                    $scope.studentprof = [];
                    $scope.studentachive = [];
                    $scope.qualificationDetails1 = [];
                    $scope.ALMST_AdmNo = promise.studentDetails[0].almsT_AdmNo;
                    $scope.studentname = promise.studentDetails[0].amsT_FirstName;
                   // $scope.fathername = promise.studentDetails[0].amsT_FatherName;
                    $('#stu').attr('src', promise.studentDetails[0].almsT_StudentPhoto);
                    $('#blahF').attr('src', promise.studentDetails[0].almsT_FamilyPhoto);
                    $scope.imageS = promise.studentDetails[0].almsT_StudentPhoto;
                    $scope.imageF = promise.studentDetails[0].almsT_FamilyPhoto;
                    $scope.obj.ASMAY_Id_Join = promise.studentDetails[0].asmaY_Id_Join;
                    $scope.obj.ASMAY_Id_Left = promise.studentDetails[0].asmaY_Id_Left;
                    $scope.obj.ASMCL_Id_Join = promise.studentDetails[0].asmcL_Id_Join;
                    $scope.obj.ASMCL_Id_Left = promise.studentDetails[0].asmcL_Id_Left;
                    $scope.ALMST_ConStreet = promise.studentDetails[0].almsT_ConStreet;
                    $scope.ALMST_ConArea = promise.studentDetails[0].almsT_ConArea;
                    $scope.ALMST_ConPincode = promise.studentDetails[0].almsT_ConPincode;
                    $scope.ALMST_BloodGroup = promise.studentDetails[0].almsT_BloodGroup;
                    $scope.almst = promise.studentDetails[0].almsT_Id;
                    $scope.yearlst = promise.fillyear;
                    $scope.classlist = promise.fillclass;
                    $scope.ALMST_PhoneNo = promise.studentDetails[0].almsT_PhoneNo;
                    $scope.ALMST_PerStreet = promise.studentDetails[0].almsT_ConStreet;
                    $scope.ALMST_PerArea = promise.studentDetails[0].almsT_ConArea;
                    $scope.arrlist = promise.countryDrpDown;
                    $scope.ALMST_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                    $scope.arrStatelist2 = promise.statedropdown;
                    $scope.arrStatelist3 = promise.statedropdown;
                    $scope.arrDistrcitlist2 = promise.districtdropdown;
                    $scope.ALMST_PerState_e = promise.studentDetails[0].almsT_ConState;
                    $scope.ALMST_FatherName = promise.studentDetails[0].almsT_FatherName;
                    $scope.ALMST_DOB = new Date(promise.studentDetails[0].almsT_DOB);                    
                    if (promise.studentDetails[0].almsT_MobileNo > 0) {
                        $scope.ALMST_MobileNo = promise.studentDetails[0].almsT_MobileNo;
                    }
                    else {
                        $scope.ALMST_MobileNo = "";
                    }
                    $scope.ALMST_MembershipId = promise.studentDetails[0].almsT_MembershipId;
                    $scope.ALMST_FullAddess = promise.studentDetails[0].almsT_FullAddess;
                    $scope.ALMST_AmountPaid = promise.studentDetails[0].almsT_AmountPaid;
                    $scope.ALMST_MembershipCategory = promise.studentDetails[0].almsT_MembershipCategory;
                    $scope.ALMST_emailId = promise.studentDetails[0].almsT_emailId;
                    $scope.ALMST_BloodGroup = promise.studentDetails[0].almsT_BloodGroup;
                    $scope.ALMST_Marital_Status = promise.studentDetails[0].almsT_Marital_Status;
                    $scope.ALMST_PerPincode = promise.studentDetails[0].almsT_ConPincode;
                    $scope.ALMST_SpouseName = promise.studentDetails[0].almsT_SpouseName;
                    $scope.ALMST_SpouseContactNo = promise.studentDetails[0].almsT_SpouseContactNo;
                    $scope.ALMST_SpouseEmailId = promise.studentDetails[0].almsT_SpouseEmailId;
                    $scope.ALMST_SpouseQulaification = promise.studentDetails[0].almsT_SpouseQulaification;
                    $scope.ALMST_SpouseProfession = promise.studentDetails[0].almsT_SpouseProfession;
                    $scope.ALMST_NickName = promise.studentDetails[0].almsT_NickName;
                    $scope.ASMAY_Id_joinyear = "";
                    angular.forEach($scope.yearlst, function (qq) {
                        if ($scope.obj.ASMAY_Id_Join == qq.asmaY_Id)
                            $scope.ASMAY_Id_joinyear = qq.asmaY_Year;
                    });
                    $scope.ASMAY_Id_Leftyear = "";
                    angular.forEach($scope.yearlst, function (qq) {
                        if ($scope.obj.ASMAY_Id_Left == qq.asmaY_Id)
                            $scope.ASMAY_Id_Leftyear = qq.asmaY_Year;
                    });
                    $scope.ASMCL_Id_Joinyear = "";
                    angular.forEach($scope.classlist, function (qq) {
                        if ($scope.obj.ASMCL_Id_Join == qq.asmcL_Id)
                            $scope.ASMCL_Id_Joinyear = qq.asmcL_ClassName;
                    });
                    $scope.ASMCL_Id_Leftyear = "";
                    angular.forEach($scope.classlist, function (qq) {
                        if ($scope.obj.ASMCL_Id_Left == qq.asmcL_Id)
                            $scope.ASMCL_Id_Leftyear = qq.asmcL_ClassName;
                    });
                    //=======prof
                    if (promise.studentprof.length > 0) {
                        $scope.ALSPR_CompanyName = promise.studentprof[0].alspR_CompanyName;
                        $scope.ALSPR_Designation = promise.studentprof[0].alspR_Designation;
                        $scope.ALSPR_WorkingSince = promise.studentprof[0].alspR_WorkingSince;
                        $scope.ALSPR_CompanyAddress = promise.studentprof[0].alspR_CompanyAddress;
                        $scope.ALSPR_EmailId = promise.studentprof[0].alspR_EmailId;
                        $scope.ALSPR_OtherDetails = promise.studentprof[0].alspR_OtherDetails;
                    }
                    else {
                        $scope.ALSPR_CompanyName = "";
                        $scope.ALSPR_Designation = "";
                        $scope.ALSPR_Designation = "";
                        $scope.ALSPR_WorkingSince = "";
                        $scope.ALSPR_CompanyAddress = "";
                        $scope.ALSPR_EmailId = "";
                        $scope.ALSPR_OtherDetails = "";
                    }
                    //===========achiev=========
                    if (promise.studentachive.length > 0) {
                        $scope.ALSAC_Achievement = promise.studentachive[0].alsaC_Achievement;
                        $scope.ALSAC_Remarks = promise.studentachive[0].alsaC_Remarks;
                    }
                    else {
                        $scope.ALSAC_Achievement = "";
                        $scope.ALSAC_Remarks = "";
                    }
                    $scope.ALMST_Id = promise.ALMST_Id;
                    $scope.placelist = promise.placelist;
                    $scope.placelistqua = [];
                    $scope.placelistqua = promise.placelistqua;
                    $scope.placelist2 = [];
                    $scope.placelist1 = [];
                    $scope.placelist3 = [];
                    if ($scope.placelistqua.length > 0 || $scope.placelistqua.length != null) {
                        var no = 0;
                        angular.forEach($scope.placelistqua, function (qq) {
                            no = no + 1;
                            $scope.placelist1.push({ id1: no, almsT_PerCitynew: qq.almsT_ConCity });
                        })
                    }
                    if ($scope.placelist.length > 0 || $scope.placelist.length != null) {
                        var no = 0;
                        angular.forEach($scope.placelist, function (qq) {
                            no = no + 1;
                            $scope.placelist2.push({ id: no, almsT_PerCity1: qq.almsT_ConCity });
                        })
                    }
                    $scope.placelist = $scope.placelist1;
                    $scope.placelist3 = $scope.placelist2;
                    //===================
                    $scope.PerCity = "";
                    $scope.ALMST_PerC = 0;
                    $scope.PerCity = promise.studentDetails[0].almsT_ConCity;
                    angular.forEach($scope.placelist2, function (qq) {
                        if ($scope.PerCity == qq.almsT_PerCity1) {
                            $scope.almsT_PerCity1 = qq.almsT_PerCity1;
                            $scope.ALMST_PerC = qq.id;
                            $scope.cityid = qq.id;
                            $scope.ALMST_PerCity3 = qq;
                            return;
                        }
                    });
                    //==========qualification
                    if (promise.studentquali.length > 0) {
                        $scope.qualificationDetails1 = [];
                        angular.forEach(promise.studentquali, function (tt) {
                            angular.forEach($scope.placelist, function (qq) {
                                if (tt.alsqU_Place != null && qq.almsT_PerCitynew != null) {
                                    if (tt.alsqU_Place.toLowerCase() == qq.almsT_PerCitynew.toLowerCase()) {
                                        $scope.almsT_PerCitynew = qq.almsT_PerCitynew;
                                        $scope.ALMST_Per = qq.id;
                                        $scope.cityid = qq.id;
                                        $scope.ALMST_PLACE = qq;
                                        return;
                                    }
                                }
                            });
                            $scope.qualificationDetails1.push({
                                Qualification: tt.alsqU_Qulification,
                                ALMST_PUC_INS_NAME: tt.alsqU_University,
                                ALMST_PUC_PASSED_OUT: tt.alsqU_YearOfPassing,
                                ALSQU_Percentage: tt.alsqU_Percentage,
                                ALMST_PLACE: tt.alsqU_Place,
                                ALMST_PerState: tt.almsT_PerState,
                                ALSQU_OtherDetails: tt.alsqU_OtherDetails,
                                placess1: false,
                                ALMST_PLACE: $scope.ALMST_PLACE
                            });

                        })

                        $scope.qualificationDetails = $scope.qualificationDetails1;
                    }


                    //==========Readmitted student details
                    $scope.readmittedDetails = [];
                }
            })
        }

        $scope.cleardata = function () {
            $state.reload();
            $scope.scroll();
        }
        $scope.clear = function () {
            $scope.ALMST_AdmNo = "";
            $scope.ALMST_PerCity3 = "";
            $scope.AMST_JOIN_YEAR = "";
            $scope.AMST_JOIN_LEFT = "";
            $scope.obj.ASMCL_Id_Join = "";
            $scope.obj.ASMCL_Id_Left = "";
            $scope.obj.ASMAY_Id_Join = "";
            $scope.obj.ASMAY_Id_Left = "";
            $scope.ASMCL_Id_Join = "";
            $scope.ASMCL_Id_Left = "";
            $scope.ALMST_PhoneNo = "";
            $scope.ALMST_MembershipId = "";
            $scope.ALMST_AmountPaid = "";
            $scope.ALMST_FullAddess = "";
            $scope.studentname = "";
            $scope.editalumni = false;
            $scope.ALMST_MembershipCategory = "";
            $scope.ALMST_PerStreet = "";
            $scope.ALMST_PerArea = "";
            $scope.arrlist = "";
            $scope.ALMST_PerCountry = "";
            $scope.arrStatelist2 = "";
            $scope.ALMST_PerState = "";
            $scope.ALMST_FatherName = "";
            $scope.ALMST_DOB = "";
            $scope.ALMST_MobileNo = "";
            $scope.ALMST_emailId = "";
            $scope.ALMST_Remarks = "";
            $scope.ALMST_PLACEnew = "";
            $scope.ALMST_PerCity = "";
            $scope.ALMST_PerPincode = "";
            $scope.ALMST_Marital_Status = "";
            $scope.ALMST_Id = "";
            $scope.ALMST_PUC_QS_DETAILS = "";
            $scope.ALMST_PUC_INS_NAME = "";
            $scope.ALMST_PUC_PASSED_OUT = "";
            $scope.ALSQU_Percentage = "";
            $scope.ALMST_PUC_PLACE = "";
            $scope.ALMST_PUC_STATE = "";
            $scope.ALMST_UG_QS_DETAILS = "";
            $scope.ALMST_UG_INS_NAME = "";
            $scope.ALMST_UG_PASSED_OUT = "";
            $scope.ALMST_UG_PERCENTAGE = "";
            $scope.ALMST_UG_PLACE = "";
            $scope.ALMST_UG_STATE = "";
            $scope.ALMST_PG_QS_DETAILS = "";
            $scope.ALMST_PG_INS_NAME = "";
            $scope.ALMST_PG_PASSED_OUT = "";
            $scope.ALMST_PG_PERCENTAGE = "";
            $scope.ALMST_PG_PLACE = "";
            $scope.ALMST_PG_STATE = "";
            $scope.ALMST_ACH_DET = "";
            $scope.ALMST_ACH_REMARKS = "";
            $scope.ALMST_PRO_COMPANY_NAME = "";
            $scope.ALMST_PRO_DESIGNATION = "";
            $scope.ALMST_PRO_OFFICE_NO = "";
            $scope.ALMST_PRO_ADDRESS = "";
            $scope.ALMST_PRO_REMARKS = "";
            $scope.ALMST_BloodGroup = "";
            $scope.ALMST_ConArea = "";
            $scope.ALMST_ConStreet = "";
            $scope.ALMST_ConPincode = "";
            $scope.ALMST_NickName = "";
            $('#stu').attr('src', "");
            $('#blahF').attr('src', "");
        }
        //Onchange Of Academic Year
        $scope.onchangeacc = function (trmA_Id) {
            $scope.studentlst = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("AlumniMembership/Getstudentlist", data).
                then(function (promise) {
                    $scope.studentlst = [];
                    $scope.studentlst1 = promise.fillstudent;
                    angular.forEach($scope.studentlst1, function (qq) {
                        $scope.studentlst.push({ almsT_Id: qq.almsT_Id, amsT_FirstName: qq.amsT_FirstName })
                    })
                    if ($scope.studentlst.length > 0) {
                        $scope.accchange = false;
                        $scope.stu_name = $scope.studentlst;
                        $scope.clear();
                    }
                    else {
                        swal('No Records Found!!');
                        $scope.clear();
                    }
                });
        };
        //Student Search Dropdown
        //Onchange Of Academic Year for Approval
        $scope.onchangeaccapp = function (trmA_Id) {
            $scope.stu_name = {};
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("AlumniMembership/Getstudentlistapp", data).
                then(function (promise) {
                    if (promise.stu_name.length > 0) {
                        //$scope.accchange = false;
                        $scope.stu_name = promise.stu_name;
                        $scope.ALSREG_Id = $scope.stu_name[0].alsreG_Id;
                    }
                    else {
                        $scope.stu_name = {};
                        $scope.ALSREG_Id = "";
                        swal('No Records Found!!');
                    }
                })
        }

        $scope.onchangecountry = function (ALMST_PerCountry) {
            var data = {
                "ALMST_PerCountry": ALMST_PerCountry
            }
            apiService.create("AlumniMembership/onchangecountry", data).               
                then(function (promise) {
                $scope.arrStatelist2 = [];
                $scope.arrStatelist3 = [];
                $scope.arrStatelist2 = promise.statedropdown;
                $scope.arrStatelist3 = promise.statedropdown;
            })
        }
        //district
        $scope.onchangestatenew = function (ALMST_PerState) {
            var data = {
                "ALMST_PerState": $scope.ALMST_PerState_e
            }
            apiService.create("AlumniMembership/onchangestate", data).
                then(function (promise) {
                    //$scope.arrStatelist2 = [];
                    //$scope.arrStatelist3 = [];
                    if (promise.districtdropdown != null && promise.districtdropdown.length > 0) {
                        $scope.arrDistrcitlist2 = promise.districtdropdown;
                  //  $scope.arrDistrictlist3 = promise.districtdropdown;
                    }
                   
                })
        }

        $scope.changecheckmatch22 = function (user) {
            $scope.listdata = [];           
            angular.forEach($scope.almstudentDetails, function (user) {
                if (user.isSelected22 == true) {
                    $scope.listdata.push(user);
                }
            })
            angular.forEach($scope.almstudentDetails, function (user) {
                user.isSelected22 = false;
            })

            angular.forEach($scope.almstudentDetails, function (dd) {
                angular.forEach($scope.listdata, function (ww) {
                    if (dd.alsreG_Id == ww.alsreG_Id) {
                        user.isSelected22 = true;
                    }
                });
            });
        };
        $scope.changecheckmatch = function (user1) {
            debugger;
            $scope.listdataaa = [];        
            angular.forEach($scope.studentDetailscheck, function (user1) {
                if (user1.isSelected == true) {
                    $scope.listdataaa.push(user1);
                }
            })
            angular.forEach($scope.studentDetailscheck, function (user1) {
                user1.isSelected = false;
            })
            angular.forEach($scope.studentDetailscheck, function (ss) {
                angular.forEach($scope.listdataaa, function (vv) {
                    if (ss.alsreG_Id == vv.alsreG_Id) {
                        user1.isSelected = true;
                    }
                });
            });
        };
        $scope.page = "page";
        $scope.reverse = true;
        $scope.page2 = "page2";
        $scope.reverse2 = true;
        $scope.currentPage = 1;
        $scope.itemsPerPages = 10;
        $scope.itemsPerPage = 10;
        $scope.currentPage2 = 1;
        $scope.itemsPerPages2 = 10;
        $scope.checkstudent = function (studentid) {
            $scope.check_list = [];
            angular.forEach($scope.almstudentDetails, function (aaa) {
                if (aaa.isSelected22 == true) {
                    $scope.check_list.push({ alsreG_Id: aaa.ALSREG_Id })
                }
            })
            var ssss = $scope.check_list[0].alsreG_Id;
            var data = {
                "ALSREG_Id": ssss
            }
            apiService.create("AlumniMembership/checkstudent", data).
                then(function (promise) {
                    if (promise.studentDetails.length > 0) {
                        //$scope.accchange = false;
                        $scope.studentDetailscheck = promise.studentDetails;
                    }
                    else {
                        $scope.studentDetailscheck = {};
                        swal('No Records Found!!');
                        $state.reload();
                    }
                })
        }
        //Student Search Dropdown
        $scope.aproovedata = function (arraystudent) {
            debugger;
            $scope.checkedlist = [];
            angular.forEach($scope.studentDetailscheck, function (cc) {
                if (cc.isSelected == true) {
                    debugger;
                    $scope.checkedlist.push({ ALSREG_Id: cc.alsreG_Id })
                }
            })
            var sssss = $scope.checkedlist[0].ALSREG_Id;
            var data = {
                "APP_FLG": $scope.Approval,
                "ALSREG_Id": sssss,
                "ALMST_Id": arraystudent[0].almsT_Id
            }
            apiService.create("AlumniMembership/aproovedata", data).
                then(function (promise) {
                    debugger;
                    if (promise.returnval == "True") {
                        swal("Alumni member successfully approved!!");
                        $state.reload();
                    }
                    else {
                        swal("Alumni member not approved!!");
                        $state.reload();
                    }
                })
        }
        $scope.aproovedatadirect = function (arraystudent) {
            $scope.checkedlist = [];
            angular.forEach($scope.almstudentDetails, function (cc) {
                if (cc.isSelected23 == true) {
                    $scope.checkedlist.push({ ALSREG_Id: cc.ALSREG_Id })
                }
            })
            var data = {
                "APP_FLG": $scope.Approval,
                "alumnistudentarray": $scope.checkedlist,
            }
            apiService.create("AlumniMembership/aproovedata", data).
                then(function (promise) {
                    debugger;
                    if (promise.returnval == "True") {
                        swal("Alumni member successfully approved!!");
                        $state.reload();
                    }
                    else {
                        swal("Alumni member not approved!!");
                        $state.reload();
                    }
                })
        }
        $scope.fulladdressadd = function (Area, Street, City, Pincode, State, Country, city1) {
            if ($scope.adminslumni == false) {
                $scope.updatedfulladdress = "";
                if (Area != undefined && Area != null && Area != "") {
                    $scope.updatedfulladdress = Area + '\r';
                }
                if (Street != undefined && Street != null && Street != "") {
                    $scope.updatedfulladdress = $scope.updatedfulladdress + Street + '\r';
                }
                if ($scope.pcity == false) {
                    if (City != undefined && City != null && City != "") {
                        if (Pincode != undefined && Pincode != null && Pincode != "") {
                            $scope.updatedfulladdress = $scope.updatedfulladdress + $scope.ALMST_PerCity3.almsT_PerCity1 + ' - ' + Pincode + '\r';
                        }
                        else {
                            $scope.updatedfulladdress = $scope.updatedfulladdress + $scope.ALMST_PerCity3.almsT_PerCity1 + '\r';
                        }
                    }
                }
                else {
                    if (city1 != undefined && city1 != null && city1 != "") {
                        if (Pincode != undefined && Pincode != null && Pincode != "") {
                            $scope.updatedfulladdress = $scope.updatedfulladdress + city1 + ' - ' + Pincode + '\r';
                        }
                        else {
                            $scope.updatedfulladdress = $scope.updatedfulladdress + city1 + '\r';
                        }
                    }
                }
                if (State != undefined && State != null && State != "") {
                    $scope.statename = "";
                    angular.forEach($scope.arrStatelist2, function (objectt) {
                        if (objectt.ivrmmS_Id === State) {
                            $scope.statename = objectt.ivrmmS_Name;
                        }
                    })
                    if (Country != undefined && Country != null && Country != "") {
                        $scope.countryname = "";
                        angular.forEach($scope.arrlist, function (objectt) {
                            if (objectt.ivrmmC_Id === Country) {
                                $scope.countryname = objectt.ivrmmC_CountryName;
                            }
                        })
                        $scope.updatedfulladdress = $scope.updatedfulladdress + $scope.statename + ', ' + $scope.countryname;
                    }
                    else {
                        $scope.updatedfulladdress = $scope.updatedfulladdress + $scope.statename;
                    }
                }

                //if (district != undefined && district != null && district != "") {
                //    $scope.districtname = "";
                //    angular.forEach($scope.arrDistrictlist2, function (objectt) {
                //        if (objectt.ivrmmD_Id === district) {
                //            $scope.districtname = objectt.ivrmmD_Name;
                //        }
                //    })
                //    if (Country != undefined && Country != null && Country != "") {
                //        $scope.countryname = "";
                //        angular.forEach($scope.arrlist, function (objectt) {
                //            if (objectt.ivrmmC_Id === Country) {
                //                $scope.countryname = objectt.ivrmmC_CountryName;
                //            }
                //        })
                //        $scope.updatedfulladdress = $scope.updatedfulladdress + $scope.statename + ', ' + $scope.countryname +',' +$scope.districtname;
                //    }
                //    else {
                //        $scope.updatedfulladdress = $scope.updatedfulladdress + $scope.districtname;
                //    }
                //}
                $scope.ALMST_FullAddess = $scope.updatedfulladdress;
            }

            $scope.onchangestatenew();

        };

        $scope.searchfilter = function (objj) {
            if (objj.search.length >= '2') {
                var data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("AlumniMembership/searchfilter", data).
                    then(function (promise) {
                        $scope.studentlst = promise.fillstudent;
                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //save
        $scope.saveData = function () {
            $scope.city = "";
            //  if ($scope.myForm.$valid) {
            $scope.almstId = "";
            //------------------------
            if ($scope.almst == undefined || $scope.almst == null || $scope.almst == '') {
                if ($scope.almsT_Id.almsT_Id != null || $scope.almsT_Id.almsT_Id != '' || $scope.almsT_Id.almsT_Id != undefined) {
                    $scope.almstId = $scope.almsT_Id.almsT_Id;
                }
                else {
                    $scope.almstId = $scope.almsT_Id;
                }
            }
            else {
                $scope.almstId = $scope.almst;
            }

            if ($scope.pcity == true) {
                $scope.city = $scope.ALMST_PerCity4;
            }
            else if ($scope.ALMST_PerCity3 != null && $scope.ALMST_PerCity3 != undefined) {
                $scope.city = $scope.ALMST_PerCity3.almsT_PerCity1;
            }
            if ($scope.ALSPR_WorkingSince == "") {
                $scope.ALSPR_WorkingSince = undefined;
            }
            if ($scope.ALSPR_EmailId == "") {
                $scope.ALSPR_EmailId = undefined;
            } if ($scope.ALMST_ConPincode == "") {
                $scope.ALMST_ConPincode = undefined;

            } if ($scope.ALMST_ConArea == "" || $scope.ALMST_ConArea == null) {
                $scope.ALMST_ConArea = undefined;
            } if ($scope.ALMST_FamilyPhoto == "" || $scope.ALMST_FamilyPhoto == null) {
                $scope.ALMST_FamilyPhoto = undefined;
            }
            if ($scope.qualificationDetails[0].Qualification != null && $scope.qualificationDetails[0].Qualification != undefined && $scope.qualificationDetails[0].Qualification != "") {
                $scope.qualificationDetails1 = [];
                angular.forEach($scope.qualificationDetails, function (qq) {
                    if (qq.placess1 == false || qq.placess1 == undefined) {
                        if (qq.ALMST_PLACE !== undefined) {
                            $scope.qualificationDetails1.push({ ALMST_PerState: qq.ALMST_PerState, ALMST_PLACE: qq.ALMST_PLACE.almsT_PerCitynew, ALMST_PUC_INS_NAME: qq.ALMST_PUC_INS_NAME, ALMST_PUC_PASSED_OUT: qq.ALMST_PUC_PASSED_OUT, ALSQU_Percentage: qq.ALSQU_Percentage, ALSQU_OtherDetails: qq.ALSQU_OtherDetails, Qualification: qq.Qualification })
                        }
                        else {
                            $scope.qualificationDetails1.push({ ALMST_PLACE: "", ALMST_PerState: qq.ALMST_PerState, ALMST_PUC_INS_NAME: qq.ALMST_PUC_INS_NAME, ALMST_PUC_PASSED_OUT: qq.ALMST_PUC_PASSED_OUT, ALSQU_Percentage: qq.ALSQU_Percentage, ALSQU_OtherDetails: qq.ALSQU_OtherDetails, Qualification: qq.Qualification })
                        }
                    }
                    else {
                        $scope.qualificationDetails1.push({ ALMST_PerState: qq.ALMST_PerState, ALMST_PLACE: qq.ALMST_PLACE7, ALMST_PUC_INS_NAME: qq.ALMST_PUC_INS_NAME, ALMST_PUC_PASSED_OUT: qq.ALMST_PUC_PASSED_OUT, ALSQU_Percentage: qq.ALSQU_Percentage, ALSQU_OtherDetails: qq.ALSQU_OtherDetails, Qualification: qq.Qualification })
                    }
                })
                $scope.qualificationDetails = $scope.qualificationDetails1
            }
            $scope.WorkingSince = 0;
            if ($scope.ALSPR_WorkingSince == undefined) {
                $scope.WorkingSince = undefined;
            }
            else {
                $scope.WorkingSince = parseInt($scope.ALSPR_WorkingSince);
            }
            if ($scope.ALMST_Marital_Status == 0) {
                $scope.ALMST_SpouseName = undefined;
                $scope.ALMST_SpouseContactNo = undefined;
                $scope.ALMST_SpouseEmailId = undefined;
                $scope.ALMST_SpouseQulaification = undefined;
                $scope.ALMST_SpouseProfession = undefined;
            }
            var almsT_PerDistrict = 0;
            if ($scope.ALMST_PerDistrict > 0) {
                almsT_PerDistrict = $scope.ALMST_PerDistrict;
            }
            var Readmitted_student = 0;
            if ($scope.obj.Readmittedstudent == true) {
                Readmitted_student = 1;
            }
            else if ($scope.obj.Readmittedstudent_check == true) {
                Readmitted_student = 1;
            }
    

            var readmitstudetails = $scope.readmitstudetails;
            $scope.readmitstudetails_array = [];

            if ($scope.editalumni == true && $scope.studentCount == 1 && $scope.obj.Readmittedstudent_check ==true) {
                if ($scope.readmitstudetails != null && $scope.readmitstudetails.length>0) {
                    angular.forEach($scope.readmitstudetails, function (qq) {

                        $scope.readmitstudetails_array.push({ ALMSTRADM_YearJoined: qq.obj.ALMSTRADM_YearJoined, ALMSTRADM_YearLeft: qq.obj.ALMSTRADM_YearLeft, ALMSTRADM_ClassJoined: qq.obj.ALMSTRADM_ClassJoined, ALMSTRADM_ClassLeft: qq.obj.ALMSTRADM_ClassLeft })
                        // $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })

                    });
                }
            }
           
         
            //-----------------
            var data = {
                "ALMST_Id": $scope.almstId,
                "ALMST_AdmNo": $scope.ALMST_AdmNo,
                "ASMAY_Id_Join": $scope.obj.ASMAY_Id_Join,
                "ASMAY_Id_Left": $scope.obj.ASMAY_Id_Left,
                "ASMCL_Id_Join": $scope.obj.ASMCL_Id_Join,
                "ASMCL_Id_Left": $scope.obj.ASMCL_Id_Left,
                "ALMST_DOB": new Date($scope.ALMST_DOB).toDateString(),
                "ALMST_MobileNo": $scope.ALMST_MobileNo,
                "ALMST_FatherName": $scope.ALMST_FatherName,
                "ALMST_PhoneNo": $scope.ALMST_PhoneNo,
                "ALMST_emailId": $scope.ALMST_emailId,
                "ALMST_Marital_Status": $scope.ALMST_Marital_Status,
                "ALMST_PerCountry": $scope.ALMST_PerCountry,
                "ALMST_PerState": $scope.ALMST_PerState_e,
                "ALMST_PerDistrict": $scope.ALMST_PerDistrict,
                "ALMST_ConCity": $scope.city,
                "ALMST_ConArea": $scope.ALMST_ConArea,
                "ALMST_ConStreet": $scope.ALMST_ConStreet,
                "ALMST_StudentPhoto": $scope.imageS,
                "ALMST_FamilyPhoto": $scope.imageF,
                "ALMST_BloodGroup": $scope.ALMST_BloodGroup,
                "ALMST_ConPincode": $scope.ALMST_ConPincode,
                "ALSAC_Achievement": $scope.ALSAC_Achievement,
                "ALSAC_Remarks": $scope.ALSAC_Remarks,
                "ALSPR_CompanyName": $scope.ALSPR_CompanyName,
                "ALSPR_Designation": $scope.ALSPR_Designation,
                "ALSPR_WorkingSince": $scope.WorkingSince,
                "ALSPR_EmailId": $scope.ALSPR_EmailId,
                "ALMST_MembershipId": $scope.ALMST_MembershipId,
                "ALMST_FullAddess": $scope.ALMST_FullAddess,
                "ALMST_AmountPaid": $scope.ALMST_AmountPaid,
                "ALMST_MembershipCategory": $scope.ALMST_MembershipCategory,
                "ALSPR_CompanyAddress": $scope.ALSPR_CompanyAddress,
                "ALSPR_OtherDetails": $scope.ALSPR_OtherDetails,
                "qualification_array": $scope.qualificationDetails,
                "ALMST_NickName": $scope.ALMST_NickName,
                "ALMST_SpouseName": $scope.ALMST_SpouseName,
                "ALMST_SpouseContactNo": $scope.ALMST_SpouseContactNo,
                "ALMST_SpouseEmailId": $scope.ALMST_SpouseEmailId,
                "ALMST_SpouseQulaification": $scope.ALMST_SpouseQulaification,
                "ALMST_SpouseProfession": $scope.ALMST_SpouseProfession,
                "ALMST_WeddingAnniversaryDate": new Date($scope.ALMST_WeddingAnniversaryDate),
                "ALMST_PerDistrict": almsT_PerDistrict,
                "Readmitted_student": Readmitted_student,
                "Readmitted_ASMAY_Id_Join": $scope.obj.readmitted_ASMAY_Id_Join,
                "Readmitted_ASMAY_Id_Left": $scope.obj.readmitted_ASMAY_Id_Left,
                "Readmitted_ASMCL_Id_Join": $scope.obj.readmitted_ASMCL_Id_Join,
                "Readmitted_ASMCL_Id_Left": $scope.obj.readmitted_ASMCL_Id_Left,
                "ReadmittedStudentDTO": $scope.readmitstudetails_array,
                "ALMSTRADM_Id": $scope.ALMSTRADM_Id


            }
            apiService.create("AlumniMembership/savedata", data).
                then(function (promise) {
                    swal(promise.returnval);
                    //$state.reload();
                    $scope.editalumni = false;
                    $scope.clear();
                    $scope.scroll();
                })
            //}
            //else {
            //    $scope.submitted = true;
            //}
        }

        $scope.svedatanewalumni = function () {
            if ($scope.myForm.$valid) {
                $scope.PerCity = "";
                if ($scope.ALMST_MiddleName == null) {
                    $scope.ALMST_MiddleName = "";
                }
                if ($scope.ALMST_LastName == null) {
                    $scope.ALMST_LastName = "";
                }
                if ($scope.pcity == true) {
                    $scope.PerCity = $scope.ALMST_PerCity3;
                }
                else {
                    if ($scope.ALMST_PerCity == undefined) {
                        $scope.PerCity = undefined;
                    }
                    else {
                        $scope.PerCity = $scope.ALMST_PerCity.almsT_PerCity1;
                    }
                }
                if ($scope.qualificationDetails[0].Qualification != null && $scope.qualificationDetails[0].Qualification != undefined && $scope.qualificationDetails[0].Qualification != "") {
                    $scope.qualificationDetails1 = [];
                    angular.forEach($scope.qualificationDetails, function (qq) {
                        if (qq.placess1 == false || qq.placess1 == undefined) {
                            if (qq.ALMST_PLACE !== undefined) {
                                $scope.qualificationDetails1.push({ ALMST_PerState: qq.ALMST_PerState, ALMST_PLACE: qq.ALMST_PLACE.almsT_PerCitynew, ALMST_PUC_INS_NAME: qq.ALMST_PUC_INS_NAME, ALMST_PUC_PASSED_OUT: qq.ALMST_PUC_PASSED_OUT, ALSQU_Percentage: qq.ALSQU_Percentage, ALSQU_OtherDetails: qq.ALSQU_OtherDetails, Qualification: qq.Qualification })
                            }
                            else {
                                $scope.qualificationDetails1.push({ ALMST_PerState: qq.ALMST_PerState, ALMST_PLACE: qq.ALMST_PLACE.almsT_PerCity, ALMST_PUC_INS_NAME: qq.ALMST_PUC_INS_NAME, ALMST_PUC_PASSED_OUT: qq.ALMST_PUC_PASSED_OUT, ALSQU_Percentage: qq.ALSQU_Percentage, ALSQU_OtherDetails: qq.ALSQU_OtherDetails, Qualification: qq.Qualification })
                            }                            
                        }
                        else {
                            $scope.qualificationDetails1.push({ ALMST_PerState: qq.ALMST_PerState, ALMST_PLACE: qq.ALMST_PLACE1, ALMST_PUC_INS_NAME: qq.ALMST_PUC_INS_NAME, ALMST_PUC_PASSED_OUT: qq.ALMST_PUC_PASSED_OUT, ALSQU_Percentage: qq.ALSQU_Percentage, ALSQU_OtherDetails: qq.ALSQU_OtherDetails, Qualification: qq.Qualification })
                        }
                    })
                    $scope.qualificationDetails = $scope.qualificationDetails1
                }
                $scope.WorkingSince = 0;
                if ($scope.ALSPR_WorkingSince == undefined) {
                    $scope.WorkingSince = undefined;
                }
                else {
                    $scope.WorkingSince = parseInt($scope.ALSPR_WorkingSince);
                }
                var data = {
                    "ALMST_FirstName": $scope.ALMST_FirstName,
                    "ALMST_MiddleName": $scope.ALMST_MiddleName,
                    "ALMST_LastName": $scope.ALMST_LastName,
                    "ALMST_FatherName": $scope.ALMST_FatherName,
                    "ALMST_DOB": new Date($scope.ALMST_DOB).toDateString(),
                    "ALMST_AdmNo": $scope.ALMST_AdmNo,
                    "ALMST_MobileNo": $scope.ALMST_MobileNo,
                    "ALMST_ConCountryId": $scope.ALMST_PerCountry,
                    "ALMST_ConState": $scope.ALMST_PerState,
                    "ALMST_ConCity": $scope.PerCity,
                    "ALMST_ConArea": $scope.ALMST_PerArea,
                    "ALMST_ConStreet": $scope.ALMST_PerStreet,
                    "ALMST_Marital_Status": $scope.ALMST_Marital_Status,
                    "ASMAY_Id_Join": $scope.obj.ASMAY_Id_Join,
                    "ASMAY_Id_Left": $scope.obj.ASMAY_Id_Left,
                    "ASMCL_Id_Join": $scope.obj.ASMCL_Id_Join,
                    "ASMCL_Id_Left": $scope.obj.ASMCL_Id_Left,
                    "ALMST_PhoneNo": $scope.ALMST_PhoneNo,
                    "ALMST_emailId": $scope.ALMST_emailId,
                    "ALMST_ConPincode": $scope.ALMST_PerPincode,
                    "ALMST_StudentPhoto": $scope.imageS,
                    "ALMST_FamilyPhoto": $scope.imageF,
                    "ALMST_BloodGroup": $scope.ALMST_BloodGroup,
                    "ALMST_NickName": $scope.ALMST_NickName,
                    "ALSAC_Achievement": $scope.ALSAC_Achievement,
                    "ALSAC_Remarks": $scope.ALSAC_Remarks,
                    "ALSPR_CompanyName": $scope.ALSPR_CompanyName,
                    "ALSPR_Designation": $scope.ALSPR_Designation,
                    "ALSPR_WorkingSince": $scope.WorkingSince,
                    "ALSPR_EmailId": $scope.ALSPR_EmailId,
                    "ALSPR_CompanyAddress": $scope.ALSPR_CompanyAddress,
                    "ALSPR_OtherDetails": $scope.ALSPR_OtherDetails,
                    "qualification_array": $scope.qualificationDetails,
                }

                apiService.create("AlumniMembership/svedatanewalumni", data).
                    then(function (promise) {
                        swal(promise.returnval);
                        $scope.scroll();
                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.aluobj = {};
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }
        $scope.student_name = "";
        $scope.edit = function (aluid, aluyear, joincls, leftcls, JoinYear, LeftYear, name, AMST_Id,ALMSTRADM_Id) {           
            $scope.studentlst = [];
            $scope.aluobj.almsT_Id = aluid;
            $scope.ASMAY_Id = aluyear;
            $scope.joincls = joincls;
            $scope.leftcls = leftcls;
            $scope.LeftYear = LeftYear;
            $scope.JoinYear = JoinYear;
            $scope.editalumni = true;            //$scope.adminslumni = false;
            //$scope.editflga = true;
            $scope.studentlst.push({ almsT_Id: aluid, amsT_FirstName: name, amsT_Id: AMST_Id })
            $scope.almst = aluid;
            $scope.amstid = AMST_Id; 
            $scope.student_name = name;
            $scope.ALMSTRADM_Id = ALMSTRADM_Id;

            $scope.onchange($scope.aluobj, $scope.amstid, $scope.ALMSTRADM_Id);
           
            
        };
        //-----student name selection change
        $scope.onchange = function (studentlst, amstid, almstradM_Id) {
            $scope.qualificationDetails = [];
            $scope.qualificationDetails = [{ Id: 'trans1', placess1: false }];
            if ($scope.qualificationDetails.length === 1) {
                $scope.cnt = 1;
            }
            var studid = studentlst;
            var stu_amst_id = amstid;
            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id
            var data = {
                "ALMST_Id": studid.almsT_Id,
                "ASMAY_Id": $scope.accyer,
                "AMST_ID": stu_amst_id,
                "ALMSTRADM_Id": almstradM_Id,
                "ASMCL_Id_Join": $scope.joincls,
                "ASMCL_Id_Left": $scope.leftcls,
                "ASMAY_Id_Join": $scope.JoinYear,
                "ASMAY_Id_Left": $scope.LeftYear
            }
            apiService.create("AlumniMembership/getstudata", data).
                then(function (promise) {
                    $scope.clear();
                    $scope.editalumni = true;
                    $scope.studentCount = promise.studentCunt;
                    $scope.studentquali = [];
                    $scope.studentprof = [];
                    $scope.studentachive = [];
                    $scope.qualificationDetails1 = [];
                    $scope.ALMST_AdmNo = promise.studentDetails[0].almsT_AdmNo;
                    $('#stu').attr('src', promise.studentDetails[0].almsT_StudentPhoto);
                    $('#blahF').attr('src', promise.studentDetails[0].almsT_FamilyPhoto);
                    $scope.imageS = promise.studentDetails[0].almsT_StudentPhoto;
                    $scope.imageF = promise.studentDetails[0].almsT_FamilyPhoto;
                    $scope.studentname = promise.studentDetails[0].amsT_FirstName;
                    $scope.ALMST_PerDistrict = promise.studentDetails[0].almsT_PerDistrict;

                    $scope.obj.Readmittedstudent = promise.readmitted_student;

                    $scope.ALMST_ConStreet = promise.studentDetails[0].almsT_ConStreet;
                    $scope.ALMST_ConArea = promise.studentDetails[0].almsT_ConArea;
                    $scope.ALMST_ConPincode = promise.studentDetails[0].almsT_ConPincode;
                    $scope.ALMST_BloodGroup = promise.studentDetails[0].almsT_BloodGroup;
                    $scope.ALMST_PhoneNo = promise.studentDetails[0].almsT_PhoneNo;
                    $scope.ALMST_PerStreet = promise.studentDetails[0].almsT_ConStreet;
                    $scope.ALMST_PerArea = promise.studentDetails[0].almsT_ConArea;
                    $scope.arrlist = promise.countryDrpDown;
                    $scope.ALMST_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                    $scope.arrStatelist2 = promise.statedropdown;
                    $scope.arrStatelist3 = promise.statedropdown;
                    $scope.ALMST_PerState_e = promise.studentDetails[0].almsT_ConState;
                    $scope.ALMST_PerDistrict = promise.studentDetails[0].almsT_PerDistrict;
                   
                    $scope.ALMST_FatherName = promise.studentDetails[0].almsT_FatherName;
                    $scope.ALMST_DOB = new Date(promise.studentDetails[0].almsT_DOB);
                    if (promise.studentDetails[0].almsT_MobileNo > 0) {
                        $scope.ALMST_MobileNo = promise.studentDetails[0].almsT_MobileNo;
                    }
                    else {
                        $scope.ALMST_MobileNo = "";
                    }                    
                    $scope.ALMST_emailId = promise.studentDetails[0].almsT_emailId;
                    $scope.ALMST_MembershipId = promise.studentDetails[0].almsT_MembershipId;
                    $scope.ALMST_AmountPaid = promise.studentDetails[0].almsT_AmountPaid;
                    $scope.ALMST_MembershipCategory = promise.studentDetails[0].almsT_MembershipCategory;
                    $scope.ALMST_FullAddess = promise.studentDetails[0].almsT_FullAddess;
                    $scope.ALMST_BloodGroup = promise.studentDetails[0].almsT_BloodGroup;
                    $scope.ALMST_Marital_Status = promise.studentDetails[0].almsT_Marital_Status;
                    $scope.ALMST_PerPincode = promise.studentDetails[0].almsT_ConPincode;
                    //$scope.ASMAY_Id_Join = promise.studentDetails[0].asmaY_Id_Join;
                    //$scope.ASMAY_Id_Left = promise.studentDetails[0].asmaY_Id_Left;
                    $scope.yearlst = promise.fillyear;
                    //$scope.ASMCL_Id_Join = promise.studentDetails[0].asmcL_Id_Join;
                    //$scope.ASMCL_Id_Left = promise.studentDetails[0].asmcL_Id_Left;

                    if ($scope.obj.Readmittedstudent == true && $scope.studentCount>1) {
                        $scope.obj.readmitted_ASMAY_Id_Join = promise.studentDetails[0].readmitted_ASMAY_Id_Join;
                        $scope.obj.readmitted_ASMAY_Id_Left = promise.studentDetails[0].readmitted_ASMAY_Id_Left;
                        $scope.obj.readmitted_ASMCL_Id_Join = promise.studentDetails[0].readmitted_ASMCL_Id_Join;
                        $scope.obj.readmitted_ASMCL_Id_Left = promise.studentDetails[0].readmitted_ASMCL_Id_Left;
                    }
                   // else {
                    $scope.obj.ASMAY_Id_Join = promise.studentDetails[0].asmaY_Id_Join;
                    $scope.obj.ASMAY_Id_Left = promise.studentDetails[0].asmaY_Id_Left;
                    $scope.obj.ASMCL_Id_Join = promise.studentDetails[0].asmcL_Id_Join;
                    $scope.obj.ASMCL_Id_Left = promise.studentDetails[0].asmcL_Id_Left;
                   // }
                    $scope.classlist = promise.fillclass;
                    $scope.ALMST_SpouseName = promise.studentDetails[0].almsT_SpouseName;
                    $scope.ALMST_SpouseContactNo = promise.studentDetails[0].almsT_SpouseContactNo;
                    $scope.ALMST_SpouseEmailId = promise.studentDetails[0].almsT_SpouseEmailId;
                    $scope.ALMST_SpouseQulaification = promise.studentDetails[0].almsT_SpouseQulaification;
                    $scope.ALMST_SpouseProfession = promise.studentDetails[0].almsT_SpouseProfession;
                    $scope.ALMST_NickName = promise.studentDetails[0].almsT_NickName;
                    $scope.ALMST_WeddingAnniversaryDate = new Date(promise.studentDetails[0].almsT_WeddingAnniversaryDate);
                    //=======prof
                    if (promise.studentprof.length > 0) {
                        $scope.ALSPR_CompanyName = promise.studentprof[0].alspR_CompanyName;
                        $scope.ALSPR_Designation = promise.studentprof[0].alspR_Designation;
                        $scope.ALSPR_WorkingSince = promise.studentprof[0].alspR_WorkingSince;
                        $scope.ALSPR_CompanyAddress = promise.studentprof[0].alspR_CompanyAddress;
                        $scope.ALSPR_EmailId = promise.studentprof[0].alspR_EmailId;
                        $scope.ALSPR_OtherDetails = promise.studentprof[0].alspR_OtherDetails;
                    }
                    else {
                        $scope.ALSPR_CompanyName = "";
                        $scope.ALSPR_Designation = "";
                        $scope.ALSPR_Designation = "";
                        $scope.ALSPR_WorkingSince = "";
                        $scope.ALSPR_CompanyAddress = "";
                        $scope.ALSPR_EmailId = "";
                        $scope.ALSPR_OtherDetails = "";
                    }
                    //===========achiev=========
                    if (promise.studentachive.length > 0) {
                        $scope.ALSAC_Achievement = promise.studentachive[0].alsaC_Achievement;
                        $scope.ALSAC_Remarks = promise.studentachive[0].alsaC_Remarks;
                    }
                    else {
                        $scope.ALSAC_Achievement = "";
                        $scope.ALSAC_Remarks = "";
                    }
                    $scope.ALMST_Id = promise.ALMST_Id;
                    $scope.placelist = promise.placelist;
                    $scope.placelistqua = [];
                    $scope.placelistqua = promise.placelistqua;
                    $scope.placelist2 = [];
                    $scope.placelist1 = [];
                    $scope.placelist3 = [];
                    if ($scope.placelistqua.length > 0 || $scope.placelistqua.length != null) {
                        var no = 0;
                        angular.forEach($scope.placelistqua, function (qq) {
                            no = no + 1;
                            $scope.placelist1.push({ id1: no, almsT_PerCitynew: qq.almsT_ConCity });
                        })
                    }
                    if ($scope.placelist.length > 0 || $scope.placelist.length != null) {
                        var no = 0;
                        angular.forEach($scope.placelist, function (qq) {
                            no = no + 1;
                            $scope.placelist2.push({ id: no, almsT_PerCity1: qq.almsT_ConCity });
                        })
                    }
                    $scope.placelist = $scope.placelist1;
                    $scope.placelist3 = $scope.placelist2;
                    //===================
                    $scope.PerCity = "";
                    $scope.ALMST_PerC = 0;
                    $scope.PerCity = promise.studentDetails[0].almsT_ConCity;
                    angular.forEach($scope.placelist2, function (qq) {
                        if ($scope.PerCity == qq.almsT_PerCity1) {
                            $scope.almsT_PerCity1 = qq.almsT_PerCity1;
                            $scope.ALMST_PerC = qq.id;
                            $scope.cityid = qq.id;
                            $scope.ALMST_PerCity3 = qq;
                            return;
                        }
                    });
                    //==========qualification
                    if (promise.studentquali.length > 0) {
                        $scope.qualificationDetails1 = [];
                        angular.forEach(promise.studentquali, function (tt) {
                            angular.forEach($scope.placelist, function (qq) {
                                if (tt.alsqU_Place != null && qq.almsT_PerCitynew != null) {
                                    if (tt.alsqU_Place.toLowerCase() == qq.almsT_PerCitynew.toLowerCase()) {
                                        $scope.almsT_PerCitynew = qq.almsT_PerCitynew;
                                        $scope.ALMST_Per = qq.id;
                                        $scope.cityid = qq.id;
                                        $scope.ALMST_PLACE = qq;
                                        return;
                                    }
                                }                               
                            });
                            $scope.qualificationDetails1.push({
                                Qualification: tt.alsqU_Qulification,
                                ALMST_PUC_INS_NAME: tt.alsqU_University,
                                ALMST_PUC_PASSED_OUT: tt.alsqU_YearOfPassing,
                                ALSQU_Percentage: tt.alsqU_Percentage,
                                ALMST_PLACE: tt.alsqU_Place,
                                ALMST_PerState: tt.almsT_PerState,
                                ALSQU_OtherDetails: tt.alsqU_OtherDetails,
                                placess1: false,
                                ALMST_PLACE: $scope.ALMST_PLACE
                            });
                        })
                        $scope.qualificationDetails = $scope.qualificationDetails1;
                    }
                    //=======================
                    $scope.scroll();
                })
        }

        $scope.toggleAll = function (e) {
            var toggleStatus = e;
            angular.forEach($scope.almstudentDetails, function (itm) {
                itm.isSelected23 = toggleStatus;
            });
        }
        $scope.optionToggled = function (user) {
            $scope.all = $scope.almstudentDetails.every(function (itm) { return itm.isSelected23; })
        }
        //====================================================================== Shivu=======

        // ADDING AND REMOVING PREVIOUS SCHOOL DETAILS
        //$scope.qualificationDetails = [{ id: 'prevSchool1' }];
        //$scope.addNewQualification = function () {
        //    var newItemNo = $scope.qualificationDetails.length + 1;
        //    if (newItemNo <= 5) {
        //        $scope.qualificationDetails.push({ 'id1': 'prevSchool' + newItemNo });
        //    }
        //};
        //$scope.removeNewQualification = function (index, data) {
        //    var newItemNo = $scope.qualificationDetails.length - 1;
        //    $scope.qualificationDetails.splice(index, 1);
        //    if (data.amstB_Id > 0) {
        //        $scope.DeletePrevSchoolData(data);
        //    }
        //    if ($scope.qualificationDetails.length === 0) {
        //        //dd
        //    }
        //};

        $scope.qualificationDetails = [{ Id: 'trans1', placess1: false }];
        if ($scope.qualificationDetails.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addNewQualification = function () {
            //$scope.submitted = true;
            //if ($scope.myForm.$valid) {
            if ($scope.qualificationDetails.length > 1) {
                for (var i = 0; i === $scope.qualificationDetails.length; i++) {
                    var id = $scope.qualificationDetails[i].Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.qualificationDetails.push({ Id: 'trans' + newItemNo, placess1: false });
        };
        $scope.removeNewQualification = function (index, data) {
            var newItemNo = $scope.qualificationDetails.length - 1;
            $scope.qualificationDetails.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }
        };
        //================ view data========
        $scope.viewData = function (user) {
            var data = {
                "ALMST_Id": user.ALMST_Id
            }
            apiService.create("AlumniMembership/viewData", data).then(function (promise) {
                if (promise.qualification > 0 || promise.profession > 0 || promise.achivement) {
                    $scope.qualification = promise.qualification;
                    $scope.profession = promise.profession;
                    $scope.achivement = promise.achivement;
                    $('#myModalCoverview').modal('show');
                }
            })
        }
        //--------Upload Father pic
        $scope.UploadStudentProfilePicS = [];
        $scope.uploadStudentProfilePicS = function (input, document) {
            $scope.UploadStudentProfilePicS = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type == "image/jpeg" || input.files[0].type == "application/pdf" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#stu')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadprofileS();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function UploadprofileS() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadStudentProfilePicS.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePicS[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    $scope.imageS = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        //--------Upload Father pic
        $scope.UploadStudentProfilePicF = [];
        $scope.uploadStudentProfilePicF = function (input, document) {
            $scope.UploadStudentProfilePicF = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahF')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadprofileF();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function UploadprofileF() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadStudentProfilePicF.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePicF[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    $scope.imageF = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }
        $scope.onchangestate1 = function (q) {
            $scope.ALMST_PerState = q;
            $scope.onchangestate();
        }
        $scope.onchangestate = function () {
            var data = {
                "ALMST_PerState": $scope.ALMST_PerState_e
            }
            apiService.create("AlumniMembership/onchangestate", data).then(function (promise) {
                $scope.placelist = [];
                $scope.placelist = promise.placelist;
                $scope.placelistqua = promise.placelistqua;
                $scope.placelist1 = [];
                $scope.placelist2 = [];
                $scope.placelist3 = [];
                if ($scope.placelistqua.length > 0 || $scope.placelistqua != null) {
                    var no = 0;
                    angular.forEach($scope.placelistqua, function (qq) {
                        no = no + 1;
                        $scope.placelist1.push({ id: no, almsT_PerCity: qq.almsT_ConCity });
                    })
                }
                if ($scope.placelist.length > 0 || $scope.placelist != null) {
                    var no = 0;
                    angular.forEach($scope.placelist, function (qq) {
                        no = no + 1;
                        $scope.placelist2.push({ id: no, almsT_PerCity1: qq.almsT_ConCity });
                    })
                }
                $scope.placelist = $scope.placelist1;
                $scope.placelist3 = $scope.placelist2;
            });
        }
        //-----------------------
        $scope.appclear = function () {
            angular.forEach($scope.almstudentDetails, function (qq) {
                qq.isSelected22 = "";
                qq.isSelected23 = "";
            })
        }
        $scope.deactive = function (item, SweetAlert) {
         
            var dystring = "";
            if (item.ALMST_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.ALMST_ActiveFlag === false) {
                dystring = "Activate";
            }
            var data =
            {
                "ALMST_Id": item.ALMST_Id,
                "ALMST_ActiveFlag": item.ALMST_ActiveFlag 
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AlumniMembership/deactive", data).
                            then(function (promise) {
                                if (promise.returnval === 'true') {
                                    swal("Record " + dystring + "d Successfully!!!");
                                    
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                // $state.reload();
                                $scope.clear();
                                $scope.loaddata();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };


        //Readmit student

        $scope.readmitstudetails = [{ id: 'readmitstudetails1' }];
        $scope.addNewReadmitstudetails = function (compExamStudetails) {
            var newItemNo = $scope.readmitstudetails.length + 1;

                if (newItemNo <= 20) {
                    $scope.readmitstudetails.push({ 'id': 'readmitstudetails' + newItemNo });
                }
        };

        $scope.removeNewReadmitstudetails = function (index, data) {
            var newItemNo = $scope.readmitstudetails.length - 1;
            $scope.readmitstudetails.splice(index, 1);
        };

        // End 



        //Akash (Edit And Deactive)
        $scope.EditAlumniHomepage = function (obj22) {
            var data = {
                "ALMST_Id": obj22.ALMST_Id
            };
            apiService.create("AlumniMembership/EditAlumniHomepages", data).then(function (promise) {
                if (promise !== null) {
                  
                    $scope.ALMST_FirstName = promise.editdatalist[0].almsT_FirstName;
                    $scope.ALMST_MiddleName = promise.editdatalist[0].almsT_MiddleName;
                    $scope.ALMST_LastName = promise.editdatalist[0].almsT_LastName;
                    $scope.ALMST_NickName = promise.editdatalist[0].almsT_NickName;
                    $scope.ALMST_AdmNo = promise.editdatalist[0].almsT_AdmNo;
                    $scope.ALMST_DOB = new Date(promise.editdatalist[0].almsT_DOB);
                    $scope.ALMST_BloodGroup = promise.editdatalist[0].almsT_BloodGroup;
                    $scope.ALMST_FatherName = promise.editdatalist[0].almsT_FatherName;
                    $scope.ALMST_Marital_Status = promise.editdatalist[0].almsT_Marital_Status;
                    $('#stu').attr('src', promise.editdatalist[0].almsT_StudentPhoto);
                    $('#blahF').attr('src', promise.editdatalist[0].almsT_FamilyPhoto);

                    $scope.ASMAY_Id_Join = promise.editdatalist[0].asmaY_Id_Join;
                    $scope.ASMAY_Id_Left = promise.editdatalist[0].asmaY_Id_Left;
                    $scope.ASMCL_Id_Join = promise.editdatalist[0].asmcL_Id_Join;
                    $scope.ASMCL_Id_Left = promise.editdatalist[0].asmcL_Id_Left;

                    $scope.ALMST_MobileNo = promise.editdatalist[0].almsT_MobileNo;
                    $scope.ALMST_PhoneNo = promise.editdatalist[0].almsT_PhoneNo;
                    $scope.ALMST_emailId = promise.editdatalist[0].almsT_emailId;
                    $scope.ALMST_PerCountry = promise.editdatalist[0].almsT_PerCountry;
                    $scope.ALMST_PerState = promise.editdatalist[0].almsT_PerState;
                    $scope.$parent.ALMST_PerCity = promise.editdatalist[0].almsT_PerCity;

                    $scope.ALMST_PerCity3 = promise.editdatalist[0].almsT_PerCity;
                    $scope.ALMST_PerArea = promise.editdatalist[0].almsT_PerArea;
                    $scope.ALMST_PerStreet = promise.editdatalist[0].almsT_PerStreet;
                    $scope.ALMST_PerPincode = promise.editdatalist[0].almsT_PerPincode;
                    $scope.ALMST_Id = promise.editdatalist[0].almsT_Id;

                    //$scope.Qualification = promise.qualificationAlmStlist[0].alsqU_Qulification;
                    //$scope.ALMST_PUC_INS_NAME = promise.qualificationAlmStlist[0].alsqU_University;
                    //$scope.ALMST_PUC_PASSED_OUT = promise.qualificationAlmStlist[0].alsqU_YearOfPassing;
                    //$scope.ALSQU_Percentage = promise.qualificationAlmStlist[0].alsqU_Percentage;
                    //$scope.ALMST_PerState = promise.qualificationAlmStlist[0].ivrmmS_Id;
                    //$scope.ALMST_PLACE = promise.qualificationAlmStlist[0].almsT_PLACE;
                    //$scope.ALMST_PLACE1 = promise.qualificationAlmStlist[0].almsT_PLACE;
                    //$scope.ALSQU_OtherDetails = promise.qualificationAlmStlist[0].alsqU_OtherDetails;

                    //$scope.qualificationDetails = promise.qualificationAlmStlist;
                    if (promise.qualificationAlmStlist.length > 0) {
                        $scope.qualificationDetails = [];
                        angular.forEach(promise.qualificationAlmStlist, function (tt) {

                            $scope.qualificationDetails.push({
                                Qualification: tt.alsqU_Qulification,
                                ALMST_PUC_INS_NAME: tt.alsqU_University,
                                ALMST_PUC_PASSED_OUT: tt.alsqU_YearOfPassing,
                                ALSQU_Percentage: tt.alsqU_Percentage,
                                ALMST_PLACE: tt.alsqU_Place,
                                ALMST_PerState: tt.almsT_PerState,
                                ALSQU_OtherDetails: tt.alsqU_OtherDetails,
                                placess1: false,
                                ALMST_PLACE: tt.almsT_PLACE
                            })


                        });
                    }





                    $scope.ALSAC_Achievement = promise.achivementALMSTDetails[0].alsaC_Achievement;
                    $scope.ALSAC_Remarks = promise.achivementALMSTDetails[0].alsaC_Remarks;

                    $scope.ALSPR_CompanyName = promise.professionaldetailslist[0].alspR_CompanyName;
                    $scope.ALSPR_Designation = promise.professionaldetailslist[0].alspR_Designation;
                    $scope.ALSPR_WorkingSince = promise.professionaldetailslist[0].alspR_WorkingSince;
                    $scope.ALSPR_CompanyAddress = promise.professionaldetailslist[0].alspR_CompanyAddress;
                    $scope.ALSPR_EmailId = promise.professionaldetailslist[0].alspR_EmailId;
                    $scope.ALSPR_OtherDetails = promise.professionaldetailslist[0].alspR_OtherDetails;
                    $scope.ALSPR_EmailId = promise.professionaldetailslist[0].alspR_EmailId;



                }
            });
        };

        $scope.AlumniHomepageActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ALMST_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ALMST_Id": obj.ALMST_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AlumniMembership/AlumniHomepageActiveDeactives", data).then(function (promise) {

                            if (promise.returnvals === true) {
                                if (promise.deactiveactivelist == true) {
                                    swal("Record Activated successfully");
                                } else {
                                    swal("Record Deactivated successfully");
                                }
                            }
                            else {
                                swal("Something Went Wrong");
                            }


                        });
                        $state.reload();
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };



    }
})();