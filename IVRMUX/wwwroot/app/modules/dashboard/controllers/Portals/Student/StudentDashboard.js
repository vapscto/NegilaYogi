
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentDashboardController', StudentDashboardController);
    StudentDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings'];

    function StudentDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings) {
        var miid = "";

        $scope.closeupdate = false;
        $scope.conformflag = true;

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });



        //***** COLOR ARRAY
        CanvasJS.addColorSet("graphcolor",
            [
                "#3498DB",
                "#76D7C4",
                "#808B96",
                "#80DEEA",
                "#C5E1A5",
                "#AAB7B8"
            ]);
        //======================================================
        $('#myCarousel').carousel({
            interval: 20000
        });
        $('.carousel .item').each(function () {
            var next = $(this).next();
            if (!next.length) {
                next = $(this).siblings(':first');
            }
            next = next.next();
            if (!next.length) {
                next = $(this).siblings(':first');
            }
        });
        //==================================================        
        var slides = $scope.slides = [];
        var currIndex = 0;
        var imagetype = '.jpg';
        $scope.addSlide = function () {
            var newWidth = 600 + slides.length + 1 + imagetype;
            slides.push({
                image: 'https://bdcampusstrg.blob.core.windows.net/files/' + miid + '/dashboardimages/' + newWidth,
                text: ['Student Portal'][slides.length % 3],
                id: currIndex++
            });
        };
        $scope.randomize = function () {
            var indexes = generateIndexesArray();
            assignNewIndexesToSlides(indexes);
        };
        for (var i = 0; i < 4; i++) {
            $scope.addSlide();
        }
        // Randomize logic below
        function assignNewIndexesToSlides(indexes) {
            for (var i = 0, l = slides.length; i < l; i++) {
                slides[i].id = indexes.pop();
            }
        }
        function generateIndexesArray() {
            var indexes = [];
            for (var i = 0; i < currIndex; ++i) {
                indexes[i] = i;
            }
            return shuffle(indexes);
        }
        $scope.feebalancecheck = false;
        //Image Slider END
        $scope.tempcldrlst = [];

        $scope.OnClickHomePage = function () {
            var data = {
            };
            apiService.create("Login/getrolewisepage", data).then(function (promise) {
                if (promise.filldashpagemap.length > 0) {
                    if (promise.pageexists.length > 0) {
                        $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName;
                    }
                    else {
                        $scope.daspgenme = "app.homepage";
                    }
                }
                else {
                    $scope.daspgenme = "app.homepage";
                }
                $state.go($scope.daspgenme);
            });
        };

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASTUREQ_Id": $scope.ASTUREQ_Id,
                    "AMSTG_Id": $scope.AMSTG_Id,
                    "Mobilenumber": $scope.Mobilenumber,
                    "EmailidforCandidate": $scope.EmailidforCandidate,
                    "AMST_FatherMobleNo": $scope.AMST_FatherMobleNo,
                    "AMST_MotherMobileNo": $scope.AMST_MotherMobileNo,
                    "AMST_MotherEmailId": $scope.AMST_MotherEmailId,
                    "AMST_FatheremailId": $scope.AMST_FatheremailId,
                    "AMSTG_GuardianPhoneNo": $scope.AMSTG_GuardianPhoneNo,
                    "AMSTG_emailid": $scope.AMSTG_emailid,

                    "STP_PERSTREET": $scope.Perstreett,
                    "STP_PERAREA": $scope.PerArea,
                    "STP_PERCITY": $scope.Percity,
                    "STP_PERSTATE": $scope.PerState,
                    "STP_PERCOUNTRY": $scope.PerCountry,
                    "STP_PERPIN": $scope.PerPincode,
                    "STP_CURSTREET": $scope.Resstreet,
                    "STP_CURAREA": $scope.ResArea,
                    "STP_CURCITY": $scope.Rescity,
                    "STP_CURSTATE": $scope.Resstate,
                    "STP_CURCOUNTRY": $scope.Rescountry,
                    "STP_CURPIN": $scope.ResPincode,
                    "AMST_BloodGroup": $scope.Bloodgroup,
                }

                swal({
                    title: "Are you sure",
                    text: "Do you want to Send  Update Request??????",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send  it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("StudentDashboard/saverequest", data).then(function (promise) {
                                if (promise.returnval == true) {
                                    $('#myModalLibrary1').modal('hide');
                                    swal('Update Request Sent Successfully..!!!');
                                    $('.modal').remove();
                                    $('.modal-backdrop').remove();
                                }
                                else {
                                    $('#myModalLibrary1').modal('hide');
                                    swal('Update Request Not Sent Successfully..!!!');
                                    $('.modal').remove();
                                    $('.modal-backdrop').remove();
                                }
                            });
                        }
                        else {
                            swal("Update Request Cancelled");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.closepopup = function () {
            $scope.closeupdate = true;
        };

        $scope.clicktext = function () {
            $scope.conformflag = true;
            if ($scope.AMST_FatherMobleNo != $scope.AMST_FatherMobleNo1) {
                $scope.conformflag = false;
            }
            if ($scope.Bloodgroup != $scope.Bloodgroup1) {
                $scope.conformflag = false;
            }
            if ($scope.Mobilenumber != $scope.Mobilenumber1) {
                $scope.conformflag = false;
            }
            if ($scope.EmailidforCandidate != $scope.EmailidforCandidate1) {
                $scope.conformflag = false;
            }

            if ($scope.AMST_FatheremailId != $scope.AMST_FatheremailId1) {
                $scope.conformflag = false;
            }

            if ($scope.AMST_MotherMobileNo != $scope.AMST_MotherMobileNo1) {
                $scope.conformflag = false;
            }
            if ($scope.AMST_MotherEmailId != $scope.AMST_MotherEmailId1) {
                $scope.conformflag = false;
            }
            if ($scope.AMSTG_GuardianPhoneNo != $scope.AMSTG_GuardianPhoneNo1) {
                $scope.conformflag = false;
            }
            if ($scope.AMSTG_emailid != $scope.AMSTG_emailid1) {
                $scope.conformflag = false;
            }
            if ($scope.Perstreett1 != $scope.Perstreett) {
                $scope.conformflag = false;
            }
            if ($scope.Percity1 != $scope.Percity) {
                $scope.conformflag = false;
            }
            if ($scope.PerArea1 != $scope.PerArea) {
                $scope.conformflag = false;
            }
            if ($scope.PerPincode1 != $scope.PerPincode) {
                $scope.conformflag = false;
            }

            if ($scope.PerCountry1 != $scope.PerCountry) {
                $scope.conformflag = false;
            }

            if ($scope.PerCountry1 != $scope.PerCountry) {
                $scope.conformflag = false;
            }
            if ($scope.PerCountryy1 != $scope.PerCountryy) {
                $scope.conformflag = false;
            }
            if ($scope.PerStatee1 != $scope.PerStatee) {
                $scope.conformflag = false;
            }
            if ($scope.Resstreet1 != $scope.Resstreet) {
                $scope.conformflag = false;
            }
            if ($scope.Rescity1 != $scope.Rescity) {
                $scope.conformflag = false;
            }
            if ($scope.ResArea1 != $scope.ResArea) {
                $scope.conformflag = false;
            }
            if ($scope.ResPincode1 != $scope.ResPincode) {
                $scope.conformflag = false;
            }
            if ($scope.Rescountryy1 != $scope.Rescountryy) {
                $scope.conformflag = false;
            }

            if ($scope.Rescountry1 != $scope.Rescountry) {
                $scope.conformflag = false;
            }
            if ($scope.Resstatee1 != $scope.Resstatee) {
                $scope.conformflag = false;
            }

            if ($scope.Resstate1 != $scope.Resstate) {
                $scope.conformflag = false;
            }
        };

        $scope.onSelectGetState2 = function (countryidd) {
            apiService.getURI("ParentsSmartCard/getdpstate", countryidd).then(function (promise) {
                $scope.statedropp = promise.stateDrpDown;
            })
            $scope.clicktext();
        }

        $scope.onSelectGetState1 = function (countryidd) {
            apiService.getURI("ParentsSmartCard/getdpstate", countryidd).then(function (promise) {
                $scope.statedrop = promise.stateDrpDown;
            })
            $scope.clicktext();
        };

        $scope.conformdata = function () {
            if ($scope.myForm.$valid) {
                var mgs = "Confirm";
                var confirmmgs = "";
                var data = {
                    "TRKMLB_Id": $scope.TRKMLB_Id,
                }

                swal({
                    title: "Are you sure",
                    text: "Do you want to " + mgs + " record??????",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("StudentDashboard/conformdata", data).then(function (promise) {
                                if (promise.returnval == true) {

                                    swal("Student Details " + 'Confirmed' + " " + "Successfully");
                                }
                                else {
                                    swal("Record " + 'Confirmation' + " Failed");
                                }
                                $('#myModalLibrary1').modal('hide');
                                $('.modal').remove();
                                $('.modal-backdrop').remove();
                                $('.modal-backdrop').remove();
                            });
                        }
                        else {
                            swal("Confirmation Cancelled");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.showtext = false;
        $scope.showtextdetails = "";
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.currentPage5 = 1;
        $scope.currentPage6 = 1;
        $scope.currentPage4 = 1;
        $scope.currentPageTT = 1;
        $scope.currentPageS = 1;
        $scope.currentPageLIB = 1;
        $scope.currentPageINV = 1;
        $scope.currentPagePDA = 1;

        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage5 = 10;
        $scope.itemsPerPage6 = 10;
        $scope.itemsPerPage4 = 10;
        $scope.itemsPerPageTT = 10;
        $scope.itemsPerPageS = 10;
        $scope.itemsPerPageLIB = 10;
        $scope.itemsPerPageINV = 10;
        $scope.itemsPerPagePDA = 10;

        $scope.loaddata = function () {
            $('#myModalLibrary1').modal('hide');

            apiService.getDATA("StudentDashboard/Getdetails").then(function (promise) {
                if (promise.pagelist !== undefined && promise.pagelist !== null && promise.pagelist.length > 0) {
                    $scope.showtextdetails = promise.pagelist[0].ivrmimP_DisplayContent;
                    if ($scope.showtextdetails !== undefined && $scope.showtextdetails !== "" && $scope.showtextdetails !== null) {
                        $scope.showtext = true;
                    }
                }
                if (promise.bookcount > 0) {

                    $scope.bookcount = promise.bookcount;

                }    //added by roopa
                if (promise.pdadetails !== null && promise.pdadetails.length > 0) {
                    $scope.pdadetails = promise.pdadetails
                    if ($scope.pdadetails !== null) {
                        for (var p = 0; p < $scope.pdadetails.length; p++) {
                            $scope.excesspaid = $scope.pdadetails[p].pdaS_CBExcessPaid;
                            $scope.studentdue = $scope.pdadetails[p].pdaS_CBStudentDue;
                        }
                    }

                }

                if (promise.inventorydetails !== null && promise.inventorydetails.length > 0) {
                    $scope.inventorydetails = promise.inventorydetails;
                    if ($scope.inventorydetails !== null) {
                        for (var b = 0; b < $scope.inventorydetails.length; b++) {
                            $scope.TotalItem = $scope.inventorydetails[b].TotalItem;
                            $scope.Salesamount = $scope.inventorydetails[b].saleAmount;
                        }
                    }
                }

                $scope.gallerlist = promise.gallerlist;
                $scope.yearlist = promise.yearlist;
                $scope.NoticeBoardYearId = promise.asmaY_Id;
                $scope.TTYearId = promise.asmaY_Id;
                $scope.SyllabusYearId = promise.asmaY_Id;
                $scope.LIBYearId = promise.asmaY_Id;
                $scope.HomwWorkYearId = promise.asmaY_Id;
                $scope.ClassWorkYearId = promise.asmaY_Id;
                $scope.InventoryYearId = promise.asmaY_Id;
                $scope.SportsYearId = promise.asmaY_Id;
                $scope.PDAYearId = promise.asmaY_Id;
                $scope.GalleryearId = promise.asmaY_Id;

                //END
                $scope.checkcount = false;
                $scope.updatestudetailslist = promise.updatestudetailslist;
                if ((promise.updatestudetailslist != undefined && promise.updatestudetailslist != null && promise.updatestudetailslist.length > 0) && $scope.closeupdate == false && promise.stdupdate === 0) {
                    $scope.countrydrop = promise.countryDrpDown;
                    $scope.statedrop = promise.stateDrpDown;
                    $scope.statedropp = promise.stateDrpDown;

                    //STUDENT DETAILS
                    $scope.NameoftheCandidate = $scope.updatestudetailslist[0].studentname;
                    $scope.EmailidforCandidate = $scope.updatestudetailslist[0].AMST_emailId;
                    $scope.EmailidforCandidate1 = $scope.updatestudetailslist[0].AMST_emailId;
                    $scope.Mobilenumber = $scope.updatestudetailslist[0].AMST_MobileNo;
                    $scope.Mobilenumber1 = $scope.updatestudetailslist[0].AMST_MobileNo;
                    $scope.Bloodgroup = $scope.updatestudetailslist[0].AMST_BloodGroup;
                    $scope.Bloodgroup1 = $scope.updatestudetailslist[0].AMST_BloodGroup;

                    //FATHER DETAILS
                    $scope.fatherName = $scope.updatestudetailslist[0].fatherName;
                    $scope.AMST_FatherMobleNo = $scope.updatestudetailslist[0].AMST_FatherMobleNo;
                    $scope.AMST_FatherMobleNo1 = $scope.updatestudetailslist[0].AMST_FatherMobleNo;
                    $scope.AMST_FatheremailId = $scope.updatestudetailslist[0].AMST_FatheremailId;
                    $scope.AMST_FatheremailId1 = $scope.updatestudetailslist[0].AMST_FatheremailId;

                    //Mother Details

                    $scope.mothername = $scope.updatestudetailslist[0].mothername;
                    $scope.AMST_MotherMobileNo = $scope.updatestudetailslist[0].AMST_MotherMobileNo;
                    $scope.AMST_MotherMobileNo1 = $scope.updatestudetailslist[0].AMST_MotherMobileNo;
                    $scope.AMST_MotherEmailId = $scope.updatestudetailslist[0].AMST_MotherEmailId;
                    $scope.AMST_MotherEmailId1 = $scope.updatestudetailslist[0].AMST_MotherEmailId;


                    //  ADDRESS

                    $scope.AMSTG_Id = $scope.updatestudetailslist[0].AMSTG_Id,

                        $scope.ASTUREQ_Id = $scope.updatestudetailslist[0].ASTUREQ_Id,
                        $scope.Perstreett = $scope.updatestudetailslist[0].AMST_PerStreet,
                        $scope.Percity = $scope.updatestudetailslist[0].AMST_PerCity,
                        $scope.PerArea = $scope.updatestudetailslist[0].AMST_PerArea,
                        $scope.PerPincode = $scope.updatestudetailslist[0].AMST_PerPincode,
                        $scope.PerCountry = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerCountryy = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerStatee = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.PerState = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.Resstreet = $scope.updatestudetailslist[0].AMST_ConStreet,
                        $scope.Rescity = $scope.updatestudetailslist[0].AMST_ConCity,
                        $scope.ResArea = $scope.updatestudetailslist[0].AMST_ConArea,
                        $scope.ResPincode = $scope.updatestudetailslist[0].AMST_ConPincode,
                        $scope.Rescountryy = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Rescountry = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Resstatee = $scope.updatestudetailslist[0].AMST_ConState,
                        $scope.Resstate = $scope.updatestudetailslist[0].AMST_ConState;



                    $scope.Perstreett1 = $scope.updatestudetailslist[0].AMST_PerStreet,
                        $scope.Percity1 = $scope.updatestudetailslist[0].AMST_PerCity,
                        $scope.PerArea1 = $scope.updatestudetailslist[0].AMST_PerArea,
                        $scope.PerPincode1 = $scope.updatestudetailslist[0].AMST_PerPincode,
                        $scope.PerCountry1 = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerCountryy1 = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerStatee1 = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.PerState1 = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.Resstreet1 = $scope.updatestudetailslist[0].AMST_ConStreet,
                        $scope.Rescity1 = $scope.updatestudetailslist[0].AMST_ConCity,
                        $scope.ResArea1 = $scope.updatestudetailslist[0].AMST_ConArea,
                        $scope.ResPincode1 = $scope.updatestudetailslist[0].AMST_ConPincode,
                        $scope.Rescountryy1 = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Rescountry1 = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Resstatee1 = $scope.updatestudetailslist[0].AMST_ConState,
                        $scope.Resstate1 = $scope.updatestudetailslist[0].AMST_ConState;

                    if ($scope.ASTUREQ_Id > 0) {

                        $scope.ASTUREQ_Date = $scope.updatestudetailslist[0].ASTUREQ_Date;
                    }
                    $('#myModalLibrary1').modal('show');
                }

                if (promise.feecheck === 0) {
                    $scope.role_type = [];
                    $scope.role_type = promise.rol_id;

                    if (promise.student_balance_list !== null && promise.student_balance_list.length > 0) {
                        if (promise.feedetails !== null) {
                            var feepending = promise.feedetails;
                            if ($scope.role_type[0].ivrmrT_Role === 'Student' || $scope.role_type[0].ivrmrT_Role === 'STUDENT') {
                                swal(feepending);
                                $scope.feebalancecheck = true;
                            }
                        }
                    }
                }

                $scope.yearfeedetails = promise.academicyearFeedata;
                $scope.yearattdetails = promise.academicyearAttendancedata;
                $scope.studetailslst = promise.studetailslist;

                $scope.coereportlst = promise.coereportlist;
                if ($scope.studetailslst != undefined && $scope.studetailslst != null && $scope.studetailslst.length > 0) {
                    $scope.AMST_Id_session = $scope.studetailslst[0].AMST_Id;

                    if ($scope.studetailslst[0].dobflg === 1) {
                        $scope.dobflg = true;
                        $('#showcake').modal('show');
                    }
                    else {
                        $scope.dobflg = false;
                        $('#showcake').modal('hide');
                        $('.modal-backdrop').remove();
                    }
                }

                // $scope.assignmentlist = promise.assignmentlist;
                $scope.imgs_list = promise.imgs_list;
                $scope.pushnotiflist = promise.pushnotiflist;
                $scope.mI_Id = promise.mI_Id;
                 $scope.homeworklist = promise.homeworklist;

                //=================
                $scope.pushnotification = promise.pushnotification;
                // $scope.sportsdetails = promise.sportsdetails;
                // $scope.librarydetails = promise.librarydetails;

                if (promise.promotionstatus !== undefined && promise.promotionstatus !== null && promise.promotionstatus.length > 0) {
                    $scope.promotionstatus = promise.promotionstatus;
                    $scope.promotionflag = true;
                    console.log($scope.promotionstatus);
                    //$scope.promotiondetails = "";
                    //if ($scope.promotionstatus[0].eprD_ClassPromoted !== null && $scope.promotionstatus[0].eprD_ClassPromoted !== '' && $scope.promotionstatus[0].eprD_ClassPromoted !== '.') {
                    //    $scope.classpromoted = " To Class " + $scope.promotionstatus[0].eprD_ClassPromoted + ".";
                    //} else {
                    //    $scope.classpromoted = ".";
                    //}
                    //$scope.promotiondetails = $scope.promotionstatus[0].eprD_PromotionName + "" + $scope.classpromoted;

                    if ($scope.promotionstatus[0].eprD_Remarks !== null && $scope.promotionstatus[0].eprD_Remarks !== "" &&
                        $scope.promotionstatus[0].eprD_Remarks !== ".") {
                        $scope.remarks = $scope.promotionstatus[0].eprD_Remarks + ".";
                    } else {
                        $scope.remarks = "";
                    }
                }
                else {
                    $scope.promotionflag = false;
                }

                $scope.addSlide();
                $scope.coeArray = [];

                //======================================= Student Details
                $scope.name = "";
                $scope.cls = "";
                $scope.sect = "";
                if ($scope.studetailslst !== undefined && $scope.studetailslst !== null && $scope.studetailslst.length > 0) {
                    for (var a = 0; a < $scope.studetailslst.length; a++) {
                        $scope.name = $scope.studetailslst[a].amsT_FirstName;
                        $scope.classnamecheck = $scope.studetailslst[a].asmcL_ClassName;
                        $scope.cls = $scope.studetailslst[a].asmcL_ClassName;
                        $scope.sect = $scope.studetailslst[a].asmC_SectionName;
                        $scope.year = $scope.studetailslst[a].asmaY_Year;
                        $scope.admno = $scope.studetailslst[a].amsT_AdmNo;
                        $scope.dob = $scope.studetailslst[a].amsT_DOB;
                        $scope.email = $scope.studetailslst[a].amsT_emailId;
                        $scope.mobileno = $scope.studetailslst[a].amsT_MobileNo;
                        $scope.regno = $scope.studetailslst[a].amsT_RegistrationNo;
                        $scope.photo = $scope.studetailslst[a].amsT_Photoname;
                        if ($scope.photo == null || $scope.photo == "") {
                            $scope.photo = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                        }
                        $scope.tpin = $scope.studetailslst[a].AMST_Tpin;
                    }
                }
                $scope.studentfeedetails = promise.studentfeedetails;
                if ($scope.studentfeedetails !== undefined && $scope.studentfeedetails !== null && $scope.studentfeedetails.length > 0) {
                    $scope.totalstudamount = promise.studentfeedetails[0].totalstudamount;
                    $scope.paidamount = promise.studentfeedetails[0].paidamount;
                    $scope.balanceamount = promise.studentfeedetails[0].balanceamount;
                }

                //=========================================== COE EVENTS
                if ($scope.coereportlst !== undefined && $scope.coereportlst !== null && $scope.coereportlst.length > 0) {
                    var totalDaysdiff = 0;
                    angular.forEach($scope.coereportlst, function (coe) {
                        var reminddate = "";
                        var ssdate = new Date(coe.coeE_EStartDate);
                        var eedate = new Date(coe.coeE_EEndDate);
                        var smonth = (ssdate.getMonth() + 1);
                        var sday = (ssdate.getDate());
                        var syear = (ssdate.getFullYear());
                        var datestart = smonth + "/" + sday + "/" + syear;
                        var emonth = (eedate.getMonth() + 1);
                        var eday = (eedate.getDate());
                        var eyear = (eedate.getFullYear());
                        var dateend = emonth + "/" + eday + "/" + eyear;
                        reminddate = new Date(coe.coeE_ReminderDate);
                        // time difference
                        var timeDiff = Math.abs(eedate.getTime() - ssdate.getTime());
                        // days difference
                        coe.diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                        totalDaysdiff += coe.diffDays;
                        $scope.totaldiffDays = totalDaysdiff;
                        if (datestart <= dateend) {
                            $scope.coeArray.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate, diffDays: coe.diffDays, coeE_Id: coe.coeE_Id
                            });
                        }
                        else if (datestart === dateend) {
                            $scope.coeArray.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate, diffDays: coe.diffDays, coeE_Id: coe.coeE_Id
                            });
                        }
                    });
                }
                //=============================================CALENDER               
                if ($scope.coereportlst !== undefined && $scope.coereportlst !== null && $scope.coereportlst.length > 0) {
                    angular.forEach($scope.coereportlst, function (qwe) {
                        qwe.title = qwe.coemE_EventName;
                        var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                        var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                        qwe.start = new Date(xyz);
                        //qwe.end = new Date(abc);
                        //$scope.tempcldrlst.push({ title: qwe.title, start: qwe.start, end: qwe.end });
                        $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                    });
                }

                //======================================= PUSHNOTIFICATION 
                $scope.currentdte = new Date();
                $scope.currentdte = $scope.currentdte === null ? "" : $filter('date')($scope.currentdte, "yyyy-MM-dd");
                $scope.pusharray = [];
                if ($scope.pushnotification !== undefined && $scope.pushnotification !== null && $scope.pushnotification.length > 0) {
                    angular.forEach(promise.pushnotification, function (pd) {
                        $scope.pushdate = new Date(pd.ipN_Date);
                        $scope.pushdate = $scope.pushdate === null ? "" : $filter('date')($scope.pushdate, "yyyy-MM-dd");

                        if ($scope.pushdate < $scope.currentdte) {
                            $scope.pusharray.push({
                                ipN_Id: pd.ipN_Id, ipN_StuStaffFlg: pd.ipN_StuStaffFlg, ipN_PushNotification: pd.ipN_PushNotification, ipN_Date: pd.ipN_Date,
                                ipnS_Id: pd.ipnS_Id, ipN_No: pd.ipN_No, amsT_Id: pd.amsT_Id, ivrmuL_Id: pd.ivrmuL_Id, amsT_FirstName: pd.amsT_FirstName
                            });
                        }
                        else if ($scope.pushdate === $scope.currentdte) {
                            $scope.pusharray.push({
                                ipN_Id: pd.ipN_Id, ipN_StuStaffFlg: pd.ipN_StuStaffFlg, ipN_PushNotification: pd.ipN_PushNotification, ipN_Date: pd.ipN_Date,
                                ipnS_Id: pd.ipnS_Id, ipN_No: pd.ipN_No, amsT_Id: pd.amsT_Id, ivrmuL_Id: pd.ivrmuL_Id, amsT_FirstName: pd.amsT_FirstName
                            });
                        }
                    });
                }

                //*************************************************PORTAL-Graph
                //#region Graph               
                $scope.total_Paid = 0;
                $scope.total_amount = 0;
                if ($scope.yearfeedetails !== undefined && $scope.yearfeedetails !== null && $scope.yearfeedetails.length > 0) {
                    for (var a = 0; a < $scope.yearfeedetails.length; a++) {
                        $scope.total_Paid += $scope.yearfeedetails[a].paid;
                        $scope.total_amount += $scope.yearfeedetails[a].Total;
                        $scope.balance += $scope.yearfeedetails[a].balance;
                    }
                }
                $scope.total_ch = 0;
                $scope.total_p = 0;
                $scope.atten = 0.00;
                if ($scope.yearattdetails !== undefined && $scope.yearattdetails !== null && $scope.yearattdetails.length > 0) {
                    for (var a = 0; a < $scope.yearattdetails.length; a++) {
                        $scope.total_ch += parseInt($scope.yearattdetails[a].CLASS_HELD);
                        $scope.total_p += $scope.yearattdetails[a].TOTAL_PRESENT;
                    }
                    $scope.atten = ($scope.total_p / $scope.total_ch) * 100;
                }
                //-----------------------Fee data
                $scope.feegraph1 = [];
                if ($scope.yearfeedetails !== undefined && $scope.yearfeedetails !== null && $scope.yearfeedetails.length > 0) {
                    for (var a = 0; a < $scope.yearfeedetails.length; a++) {
                        $scope.feegraph1.push({ label: $scope.yearfeedetails[a].frommonth + '-' + $scope.yearfeedetails[a].tomonth, "y": $scope.yearfeedetails[a].Total })
                    }
                }
                console.log($scope.feegraph1);
                $scope.feegraph2 = [];
                if ($scope.yearfeedetails !== undefined && $scope.yearfeedetails !== null && $scope.yearfeedetails.length > 0) {
                    for (var a = 0; a < $scope.yearfeedetails.length; a++) {
                        $scope.feegraph2.push({ label: $scope.yearfeedetails[a].frommonth + '-' + $scope.yearfeedetails[a].tomonth, "y": $scope.yearfeedetails[a].paid })
                    }
                }
                console.log($scope.feegraph2);
                //-----------------------Fee Graph

                var chart = new CanvasJS.Chart("rangeBarChat", {
                    animationEnabled: true,
                    animationDuration: 3000,
                    height: 350,
                    colorSet: "graphcolor",
                    axisX: {
                        labelFontSize: 13,
                    },
                    axisY: {
                        labelFontSize: 13,
                    },

                    toolTip: {
                        shared: true
                    },
                    data: [{
                        name: "TOTAL AMOUNT",
                        showInLegend: true,
                        type: "column",
                        // color: "rgba(40,175,101,0.6)",
                        dataPoints: $scope.feegraph1
                    },
                    {
                        name: "PAID AMOUNT",
                        showInLegend: true,
                        type: "column",
                        //  color: "rgba(0,75,141,0.7)",
                        dataPoints: $scope.feegraph2
                    }]
                });
                chart.render();
                //---End

                //-----------------------Attendance data        
                $scope.attegraph1 = [];
                if ($scope.yearattdetails !== undefined && $scope.yearattdetails !== null && $scope.yearattdetails.length > 0) {
                    for (var a = 0; a < $scope.yearattdetails.length; a++) {
                        $scope.attegraph1.push({ label: $scope.yearattdetails[a].MONTH_NAME, "y": parseInt($scope.yearattdetails[a].CLASS_HELD) })
                    }
                }

                $scope.attegraph2 = [];
                if ($scope.yearattdetails !== undefined && $scope.yearattdetails !== null && $scope.yearattdetails.length > 0) {
                    for (var a = 0; a < $scope.yearattdetails.length; a++) {
                        $scope.attegraph2.push({ label: $scope.yearattdetails[a].MONTH_NAME, "y": $scope.yearattdetails[a].TOTAL_PRESENT })
                    }
                }

                //-------------------Attendance Graph   
                var chart1 = new CanvasJS.Chart("chartContainer", {
                    animationEnabled: true,
                    animationDuration: 3000,
                    height: 350,
                    colorSet: "graphcolor",
                    axisX: {
                        labelFontSize: 13,
                    },
                    axisY: {
                        labelFontSize: 13,
                    },

                    toolTip: {
                        shared: true
                    },
                    data: [{
                        name: "CLASS HELD",
                        showInLegend: true,
                        type: "column",
                        // color: "rgba(40,175,101,0.6)",
                        dataPoints: $scope.attegraph1
                    },
                    {
                        name: "TOTAL PRESENT",
                        showInLegend: true,
                        type: "column",
                        //  color: "rgba(0,75,141,0.7)",
                        dataPoints: $scope.attegraph2
                    }]
                });
                chart1.render();
                if ($scope.yearlist !== undefined && $scope.yearlist !== null && $scope.yearlist.length > 0) {
                    angular.forEach($scope.yearlist, function (ss) {
                        if (ss.asmaY_Id === promise.asmaY_Id) {
                            $scope.year_name_att = ss.asmaY_Year;
                            $scope.year_name_fee = ss.asmaY_Year;
                        }
                    });
                }

                $scope.showonlineexam = false;
                $scope.gettodaysexamdetails = promise.gettodaysexamdetails;

                if ($scope.gettodaysexamdetails !== undefined && $scope.gettodaysexamdetails !== null && $scope.gettodaysexamdetails.length > 0) {
                    $scope.showonlineexam = true;

                    if (promise.stuonlineexam === 0) {
                        $('#mymodalonlineexam').modal('show');
                    }
                }
            });
        };


        $scope.RedirectToOE = function () {
            window.location.href = "http://localhost:57606/#/app/LP_OnlineStudentExam/915";
            $('.modal-backdrop').remove();
        };

        //============ onchange function

        $scope.onclick_notice = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',

            };
            apiService.create("StudentDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount, ASMCL_Id: nt.asmcL_Id, INTBCSTDV_ViewFlag: nt.INTBCSTDV_ViewFlag });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalNotice').modal('show');
            });
        };

        $scope.OnChangeNoticeboardYear = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.NoticeBoardYearId,
                "OnClickOrOnChange": 'OnChange'
            };

            apiService.create("StudentDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_TT = function (flag) {
            $scope.noticeTT = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick'
            };
            apiService.create("StudentDashboard/onclick_TT", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeTT = [];

                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeTT.push({ intB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, NTB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalExamTT').modal('show');
            });
        };

        $scope.OnChangeTTYear = function (flag) {
            $scope.noticeTT = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnChange',
                "ASMAY_Id": $scope.TTYearId

            };
            apiService.create("StudentDashboard/onclick_TT", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeTT = [];

                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeTT.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, NTB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_syllabus = function (flag) {
            $scope.noticeSyllabus = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_syllabus", data).then(function (promise) {

                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeSyllabus = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeSyllabus.push({ intB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalSyllabus').modal('show');
            });
        };

        $scope.OnChangeSyllabusYear = function (flag) {
            $scope.noticeSyllabus = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnChange',
                "ASMAY_Id": $scope.SyllabusYearId
            };
            apiService.create("StudentDashboard/onclick_syllabus", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeSyllabus = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeSyllabus.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    });
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_LIB = function (flag) {
            $scope.librarydetails = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_LIB", data).then(function (promise) {
                if (promise.librarydetails !== null && promise.librarydetails.length > 0) {
                    $scope.librarydetails = promise.librarydetails;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalLibrary').modal('show');
            });
        };

        $scope.OnChangeLIBYear = function (flag) {
            $scope.librarydetails = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.LIBYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_LIB", data).then(function (promise) {
                if (promise.librarydetails !== null && promise.librarydetails.length > 0) {
                    $scope.librarydetails = promise.librarydetails;                   
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalLibrary').modal('show');
            });
        };

        $scope.onclick_Homework = function (flag) {
            $scope.homeworklist = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick'
            };
            apiService.create("StudentDashboard/onclick_Homework", data).then(function (promise) {
                if (promise.homeworklist !== null && promise.homeworklist.length > 0) {
                    $scope.homeworklist = promise.homeworklist;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalhomeworkd1').modal('show');
            });
        };


        //added by roopa classwork and homework
        $scope.viewDetail_homework = function (flag) {
            //$scope.homeworklist = [];
            // $scope.updaterow(flag);

            var data = {
                "flag": 'Homework',
                "IHW_Id": flag.ihW_Id,
                "OnClickOrOnChange": 'OnClick',
                "ASMCL_Id": flag.asmcL_Id,
                "ASMS_Id": flag.asmS_Id
            };
            apiService.create("StudentDashboard/onclick_Homework_seen", data).then(function (promise) {
                if (promise.homeworklist_byid !== null && promise.homeworklist_byid.length > 0) {
                    $scope.homeworklist_byid = promise.homeworklist_byid;
                }
                //else {
                //    swal('No Data Found..!!');
                //}


                $('#myModalhomeworkd_seen').modal('show');

            });
        };



              //added by roopa staff details
        $scope.viewStaffDetail = function (hrmeid) {


            var data = {
                "HRME_Id": hrmeid
            };
            apiService.create("StudentDashboard/onclick_Staff_details", data).then(function (promise) {
                if (promise.staffdetails !== null && promise.staffdetails.length > 0) {
                    $scope.staffdetails = promise.staffdetails;
                }
                //else {
                //    swal('No Data Found..!!');
                //}


                $('#modal_staffProfile').modal('show');

            });
        };


        $scope.viewDetail_classwork = function (flag) {
            //$scope.homeworklist = [];
            var data = {
                "flag": 'Classwork',
                "ICW_Id": flag.icW_Id,
                "OnClickOrOnChange": 'OnClick',
                "ASMCL_Id": flag.ASMCL_Id,
                "ASMS_Id": flag.ASMS_Id
            };
            apiService.create("StudentDashboard/onclick_classwork_seen", data).then(function (promise) {
                if (promise.classworklist_byid != null && promise.classworklist_byid.length > 0) {
                    $scope.classworklist_byid = promise.classworklist_byid;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalAssignment1_seen').modal('show');
            });
        };


        $scope.viewDetail_noticeboard = function (flag) {
            //$scope.homeworklist = [];
            var data = {
                "flag": 'O',
                //"INTB_Id": flag,
                //"OnClickOrOnChange": 'OnClick'

                "INTB_Id": flag.INTB_Id,
                "OnClickOrOnChange": 'OnClick',
                //"ASMCL_Id": flag.ASMCL_Id

            };
            apiService.create("StudentDashboard/onclick_noticeboard_seen", data).then(function (promise) {
                if (promise.noticelist_byid != null && promise.noticelist_byid.length > 0) {
                    $scope.noticelist_byid = promise.noticelist_byid;
                    //$scope.noticeboard = [];
                    //angular.forEach($scope.noticelist, function (nt) {
                    //    $scope.noticeboard.push({ INTB_Id: nt.intB_Id, INTB_Title: nt.intB_Title, ntB_TTSylabusFlg: nt.intB_TTSylabusFlg, INTB_Attachment: nt.intB_Attachment, INTB_StartDate: nt.intB_StartDate, INTB_EndDate: nt.intB_EndDate, INTB_FilePath: nt.intB_FilePath, INTB_Description: nt.intB_Description, Filecount: nt.Filecount });
                    //});
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalNotice_seen').modal('show');
            });
        };
        //

        $scope.OnChangeHWYear = function (flag) {
            $scope.homeworklist = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.HomwWorkYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_Homework", data).then(function (promise) {
                if (promise.homeworklist !== null && promise.homeworklist.length > 0) {
                    $scope.homeworklist = promise.homeworklist;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_Classwork = function (flag) {
            $scope.assignmentarray = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_Classwork", data).then(function (promise) {
                if (promise.assignmentlist !== null && promise.assignmentlist.length > 0) {
                    $scope.assignmentlist = promise.assignmentlist;
                    $scope.studetailslst = promise.studetailslist;
                    $scope.assignmentarray = [];
                    $scope.asgnarray = [];
                    $scope.nowdate = new Date();
                    $scope.nowdate = $scope.nowdate === null ? "" : $filter('date')($scope.nowdate, "yyyy-MM-dd");
                    if ($scope.assignmentlist.length > 0) {
                        angular.forEach($scope.studetailslst, function (stdlst) {
                            angular.forEach($scope.assignmentlist, function (asgn) {
                                if (stdlst.asmcL_Id === asgn.asmcL_Id) {

                                    $scope.assignmentarray.push({
                                        icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate,
                                        icW_Topic: asgn.icW_Topic, icW_SubTopic: asgn.icW_SubTopic, icW_Content: asgn.icW_Content,
                                        icW_TeachingAid: asgn.icW_TeachingAid, icW_Assignment: asgn.icW_Assignment, icW_Evaluation: asgn.icW_Evaluation,
                                        icW_Attachment: asgn.icW_Attachment, icW_FilePath: asgn.icW_FilePath, nowdate: $scope.nowdate,
                                        ismS_SubjectName: asgn.ismS_SubjectName, icW_Id: asgn.icW_Id, icwupL_Marks: asgn.icwupL_Marks, FilesCount: asgn.FilesCount, ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg, ASMCL_Id: asgn.asmcL_Id
                                        , ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg, ASMS_Id: asgn.asmS_Id
                                    });

                                    var curdate = new Date();
                                    var asgnmddate = new Date(asgn.icW_ToDate);
                                    asgnmddate.setDate(asgnmddate.getDate());

                                    var smonth = (curdate.getMonth() + 1);
                                    var sday = (curdate.getDate());
                                    var syear = (curdate.getFullYear());
                                    var sdate = smonth + "/" + sday + "/" + syear;

                                    var emonth = (asgnmddate.getMonth() + 1);
                                    var eday = (asgnmddate.getDate());
                                    var eyear = (asgnmddate.getFullYear());
                                    var edate = emonth + "/" + eday + "/" + eyear;

                                    if (sdate <= edate) {
                                        $scope.asgnarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate, icW_Topic: asgn.icW_Topic, icW_Assignment: asgn.icW_Assignment, icW_Id: asgn.icW_Id, ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg });
                                    }
                                    else if (sdate === edate) {
                                        $scope.asgnarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate, icW_Topic: asgn.icW_Topic, icW_Assignment: asgn.icW_Assignment, icW_Id: asgn.icW_Id, ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg });
                                    }
                                    else if ($scope.asgnarray.length === 0) {
                                        $scope.assgnwork = "Not Given Any Assignment!";
                                    }
                                }
                                else {
                                    $scope.assgnwork = "Not Given Any Assignment!";
                                }
                            });
                            $scope.assignmentarray = $scope.assignmentarray;
                        });
                    }
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalAssignment1').modal('show');
            });
        };

        $scope.OnChangeCWYear = function (flag) {
            $scope.assignmentarray = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.ClassWorkYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_Classwork", data).then(function (promise) {
                if (promise.assignmentlist !== null && promise.assignmentlist.length > 0) {
                    $scope.assignmentlist = promise.assignmentlist;
                    $scope.studetailslst = promise.studetailslist;
                    $scope.assignmentarray = [];
                    $scope.asgnarray = [];
                    $scope.nowdate = new Date();
                    $scope.nowdate = $scope.nowdate === null ? "" : $filter('date')($scope.nowdate, "yyyy-MM-dd");
                    if ($scope.assignmentlist.length > 0) {
                        angular.forEach($scope.studetailslst, function (stdlst) {
                            angular.forEach($scope.assignmentlist, function (asgn) {
                                if (stdlst.asmcL_Id === asgn.asmcL_Id) {

                                    $scope.assignmentarray.push({
                                        icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate,
                                        icW_Topic: asgn.icW_Topic, icW_SubTopic: asgn.icW_SubTopic, icW_Content: asgn.icW_Content,
                                        icW_TeachingAid: asgn.icW_TeachingAid, icW_Assignment: asgn.icW_Assignment, icW_Evaluation: asgn.icW_Evaluation,
                                        icW_Attachment: asgn.icW_Attachment, icW_FilePath: asgn.icW_FilePath, nowdate: $scope.nowdate,
                                        ismS_SubjectName: asgn.ismS_SubjectName, icW_Id: asgn.icW_Id, icwupL_Marks: asgn.icwupL_Marks, FilesCount: asgn.FilesCount, asmcL_Id: asgn.asmcL_Id
                                        , ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg, ASMS_Id: asgn.asmS_Id
                                    });

                                    var curdate = new Date();
                                    var asgnmddate = new Date(asgn.icW_ToDate);
                                    asgnmddate.setDate(asgnmddate.getDate());

                                    var smonth = (curdate.getMonth() + 1);
                                    var sday = (curdate.getDate());
                                    var syear = (curdate.getFullYear());
                                    var sdate = smonth + "/" + sday + "/" + syear;

                                    var emonth = (asgnmddate.getMonth() + 1);
                                    var eday = (asgnmddate.getDate());
                                    var eyear = (asgnmddate.getFullYear());
                                    var edate = emonth + "/" + eday + "/" + eyear;

                                    if (sdate <= edate) {
                                        $scope.asgnarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate, icW_Topic: asgn.icW_Topic, icW_Assignment: asgn.icW_Assignment, icW_Id: asgn.icW_Id, asmcL_Id: asgn.asmcL_Id, ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg });
                                    }
                                    else if (sdate === edate) {
                                        $scope.asgnarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate, icW_Topic: asgn.icW_Topic, icW_Assignment: asgn.icW_Assignment, icW_Id: asgn.icW_Id, asmcL_Id: asgn.asmcL_Id, ICWUPL_ViewedFlg: asgn.ICWUPL_ViewedFlg });
                                    }
                                    else if ($scope.asgnarray.length === 0) {
                                        $scope.assgnwork = "Not Given Any Assignment!";
                                    }
                                }
                                else {
                                    $scope.assgnwork = "Not Given Any Assignment!";
                                }
                            });

                            $scope.assignmentarray = $scope.assignmentarray;
                        });
                    }
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_Sports = function (flag) {
            $scope.sportsdetails = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_Sports", data).then(function (promise) {
                if (promise.sportsdetails !== null && promise.sportsdetails.length > 0) {
                    $scope.sportsdetails = promise.sportsdetails;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalSports').modal('show');
            });
        };

        $scope.OnChangeSportsYear = function (flag) {
            $scope.sportsdetails = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.SportsYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_Sports", data).then(function (promise) {
                if (promise.sportsdetails !== null && promise.sportsdetails.length > 0) {
                    $scope.sportsdetails = promise.sportsdetails;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_Inventory = function (flag) {
            $scope.inventorydetails = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_Inventory", data).then(function (promise) {

                if (promise.inventorydetails !== null && promise.inventorydetails.length > 0) {
                    $scope.inventorydetails = promise.inventorydetails;
                    if ($scope.inventorydetails !== null) {
                        for (var b = 0; b < $scope.inventorydetails.length; b++) {
                            $scope.TotalItem = $scope.inventorydetails[b].TotalItem;
                            $scope.Salesamount = $scope.inventorydetails[b].saleAmount;
                        }
                    }
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalInventory').modal('show');
            });
        };

        $scope.OnChangeInventoryYear = function (flag) {
            $scope.inventorydetails = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.InventoryYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_Inventory", data).then(function (promise) {

                if (promise.inventorydetails !== null && promise.inventorydetails.length > 0) {
                    $scope.inventorydetails = promise.inventorydetails;
                    if ($scope.inventorydetails !== null) {
                        for (var b = 0; b < $scope.inventorydetails.length; b++) {
                            $scope.TotalItem = $scope.inventorydetails[b].TotalItem;
                            $scope.Salesamount = $scope.inventorydetails[b].saleAmount;
                        }
                    }
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_PDA = function (flag) {
            $scope.pdadetails = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_PDA", data).then(function (promise) {
                if (promise.pdadetails !== null && promise.pdadetails.length > 0) {
                    $scope.pdadetails = promise.pdadetails;
                    if ($scope.pdadetails !== null) {
                        for (var p = 0; p < $scope.pdadetails.length; p++) {
                            $scope.excesspaid = $scope.pdadetails[p].pdaS_CBExcessPaid;
                            $scope.studentdue = $scope.pdadetails[p].pdaS_CBStudentDue;
                        }
                    }
                }
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalPDA').modal('show');
            });
        };

        $scope.OnChangePDAYear = function (flag) {
            $scope.pdadetails = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.PDAYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_PDA", data).then(function (promise) {
                if (promise.pdadetails !== null && promise.pdadetails.length > 0) {
                    $scope.pdadetails = promise.pdadetails;
                    if ($scope.pdadetails !== null) {
                        for (var p = 0; p < $scope.pdadetails.length; p++) {
                            $scope.excesspaid = $scope.pdadetails[p].pdaS_CBExcessPaid;
                            $scope.studentdue = $scope.pdadetails[p].pdaS_CBStudentDue;
                        }
                    }
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };

        $scope.onclick_Gallery = function (flag) {
            $scope.GalleryearId = "";
            var data = {
                "OnClickOrOnChange": 'OnClick',
                "flag": flag
            };
            apiService.create("StudentDashboard/onclick_Gallery", data).then(function (promise) {
                if (promise.imagegallery !== null && promise.imagegallery.length > 0) {
                    $scope.imagegallery = promise.imagegallery;                    
                }
                $scope.imagegallery = [];
                //else {
                //    swal('No Data Found..!!');
                //}
                $('#myModalCoverww').modal('show');
            });
        };


        $scope.onclick_Displaymessage = function () {
            var data = {
                "msgdis": $scope.msgdis = 1
            };
            apiService.create("StudentDashboard/onclick_Displaymessage", data).then(function (promise) {
                if (promise.displyamessage !== null && promise.displyamessage.length > 0) {
                    $scope.displyamessages = promise.displyamessage;
                    $('#myModalDiaplymessg').modal('show');
                }
                else {
                    swal('No Data Found..!!');
                }

            });
        };


        $scope.OnChangeGALLERYYear = function (flag) {
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.GalleryearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_Gallery", data).then(function (promise) {
                if (promise.imagegallery !== null && promise.imagegallery.length > 0) {
                    $scope.imagegallery = promise.imagegallery;
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };
        $scope.OnChangeGALLERY = function (IGA_Id) {
            $scope.imagegallery = [];
            var data = {
                //"flag": flag,
                IGA_Id: IGA_Id,
                "OnClickOrOnChange": 'OnChange'
            };
            apiService.create("StudentDashboard/onclick_Gallery", data).then(function (promise) {
                if (promise.imagegallery !== null && promise.imagegallery.length > 0) {
                    $scope.imagegallery = promise.imagegallery;
                }
                $('#myModalG').modal('show');
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };


        $scope.imgurll = '';
        $scope.imgshow = false;
        $scope.vidshow = false;

        $scope.viewimage = function (dd) {
            $scope.imgshow = false;
            $scope.vidshow = false;
            if (dd.IGAP_Id != null) {
                $scope.imgshow = true;
            }
            if (dd.IGAV_Id != null) {
                $scope.vidshow = true;
            }
            $scope.imgurll = dd.IGAP_Photos;
            $scope.viwvideosss = dd.IGAV_Videos;
            $('#myModalCoverww1').modal('show');
        };

        $scope.loadhomeworkdata = function () {
            $scope.homeworkuploadedlist = [];
            $scope.homeworknotuploadedlist = [];
            $scope.myTabIndex = 0;
            var data = {
                "flag": "Homework",
                "OnClickOrOnChange": 'OnClick',
            };
            apiService.create("StudentDashboard/onclick_Homework_load", data).then(function (promise) {

                if (promise.homeworklist !== null && promise.homeworklist.length > 0) {
                    $scope.homeworklist = promise.homeworklist;

                    $scope.homeworkuploadedlist = [];
                    $scope.homeworknotuploadedlist = [];
                    angular.forEach($scope.homeworklist, function (dd) {
                        if (dd.student_upload > 0) {
                            $scope.homeworkuploadedlist.push(dd);
                        } else {
                            $scope.homeworknotuploadedlist.push(dd);
                        }
                    });

                    $scope.AMST_Id = promise.amsT_Id;
                    $('#myModalhomeworkd1').modal('show');
                }
                //else {
                //    swal('No Data Found..!!');
                //}

                $scope.yearlist_home = promise.yearlist;
                $scope.ASMAY_Id_Temp = promise.asmaY_Id;

            });
        };

        $scope.OnChangehomeworkYear = function () {

            $scope.homeworkuploadedlist = [];
            $scope.homeworknotuploadedlist = [];
            $scope.myTabIndex = 0;
            var data = {
                "flag": "Homework",
                "ASMAY_Id": $scope.ASMAY_Id_Temp,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_Homework_load", data).then(function (promise) {

                if (promise.homeworklist !== null && promise.homeworklist.length > 0) {
                    $scope.homeworklist = promise.homeworklist;

                    $scope.homeworkuploadedlist = [];
                    $scope.homeworknotuploadedlist = [];
                    angular.forEach($scope.homeworklist, function (dd) {
                        if (dd.student_upload > 0) {
                            $scope.homeworkuploadedlist.push(dd);
                        } else {
                            $scope.homeworknotuploadedlist.push(dd);
                        }
                    });

                    $scope.AMST_Id = promise.amsT_Id;
                    $('#myModalhomeworkd1').modal('show');
                }
                //else {
                //    swal('No Data Found..!!');
                //}

                $scope.yearlist_home = promise.yearlist;
                $scope.ASMAY_Id_Temp = promise.asmaY_Id;
            });
        };



        //ADDED BY CHETHAN

        $scope.onclick_booklist = function (flag) {
            $scope.booklist = [];
            $('#myModalbooklist').modal('hide');
            var data = {
                "flag": "BOOK"
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.booklist = promise.assetlist;
                    $('#myModalbooklist').modal('show');

                }
                else {
                    swal('No Data Found..!!');

                }

            }
            );
        };

        //$scope.onclick_booklistwo = function (flag) {
        //    $scope.bookFillist = [];

        //    var data = {
        //        "flag": "BOOKLIST"
        //    };
        //    apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
        //        if (promise.assetlist !== null && promise.assetlist.length > 0) {
        //            $scope.bookFillist = promise.assetlist;
                
        //        }
        //        else if (promise.displyamessage.length > 0) {
        //            $scope.displyamessages = $scope.displyamessage
        //        }

        //        else {
        //            swal('No Data Found..!!');

        //        }

        //    }
        //    );
        //};

        $scope.loadclassworkdata = function () {
            $scope.classworkuploadedlist = [];
            $scope.classworknotuploadedlist = [];
            $scope.myTabIndex = 0;
            var data = {
                "flag": "Classwork",
                "OnClickOrOnChange": 'OnClick'
            };
            apiService.create("StudentDashboard/onclick_Classwork_load", data).then(function (promise) {

                if (promise.assignmentlist !== null && promise.assignmentlist.length > 0) {
                    $scope.AMST_Id = promise.amsT_Id;
                    $scope.assignmentarray = promise.assignmentlist;
                    $scope.studetailslst = promise.studetailslist;
                    $scope.classworkuploadedlist = [];
                    $scope.classworknotuploadedlist = [];

                    angular.forEach($scope.assignmentarray, function (dd) {
                        if (dd.student_upload === 0) {
                            $scope.classworknotuploadedlist.push(dd);
                        } else if (dd.icW_FilePath === 1) {
                            $scope.classworkuploadedlist.push(dd);
                        }
                    });

                    $scope.nowdate = new Date();
                    $scope.nowdate = $scope.nowdate === null ? "" : $filter('date')($scope.nowdate, "yyyy-MM-dd");
                    $('#myModalAssignment1').modal('show');
                }
                //else {
                //    swal('No Data Found..!!');
                //}

                $scope.yearlist_class = promise.yearlist;
                $scope.ASMAY_Id_Temp = promise.asmaY_Id;

            });
        };

        $scope.OnChangeClassworkYear = function () {
            $scope.classworkuploadedlist = [];
            $scope.classworknotuploadedlist = [];
            $scope.myTabIndex = 0;
            var data = {
                "flag": "Classwork",
                "OnClickOrOnChange": 'OnChange',
                "ASMAY_Id": $scope.ASMAY_Id_Temp
            };
            apiService.create("StudentDashboard/onclick_Classwork_load", data).then(function (promise) {

                if (promise.assignmentlist !== null && promise.assignmentlist.length > 0) {
                    $scope.AMST_Id = promise.amsT_Id;
                    $scope.assignmentarray = promise.assignmentlist;
                    $scope.studetailslst = promise.studetailslist;
                    $scope.classworkuploadedlist = [];
                    $scope.classworknotuploadedlist = [];

                    angular.forEach($scope.assignmentarray, function (dd) {
                        if (dd.icW_FilePath === 0) {
                            $scope.classworknotuploadedlist.push(dd);
                        } else if (dd.icW_FilePath === 1) {
                            $scope.classworkuploadedlist.push(dd);
                        }
                    });

                    $scope.nowdate = new Date();
                    $scope.nowdate = $scope.nowdate === null ? "" : $filter('date')($scope.nowdate, "yyyy-MM-dd");
                    $('#myModalAssignment1').modal('show');
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };


        //================================PORTAL-CALENDER
        // #region PortalCalender
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        $scope.changeTo = 'Hungarian';
        $scope.currentView = 'month';

        /* event source that contains custom events on the scope */
        $scope.events = $scope.tempcldrlst;
        /* event source that calls a function on every view switch */
        $scope.eventsF = function (start, end, timezone, callback) {

            var s = new Date(start).getTime() / 1000;
            //  var e = new Date(end).getTime() / 1000;
            var m = new Date(start).getMonth();
            var events = [{
                title: 'Feed Me ' + m,
                start: s + (50000),
                // end: s + (100000),
                allDay: false,
                className: ['customFeed']
            }];
            callback(events);
        };
        $scope.calEventsExt = {
            color: '#f00',
            textColor: 'yellow',
            events: []
        };
        $scope.ev = {};
        /* alert on dayClick */
        $scope.alertOnDayClick = function (date) {
            $scope.alertMessage = (date.toString() + ' was clicked ');
            $scope.ev = {
                from: date.format('YYYY-MM-DD'),
                to: date.format('YYYY-MM-DD'),
                title: '',
                allDay: true

            };
        };
        /* alert on eventClick */
        $scope.alertOnEventClick = function (date, jsEvent, view) {
            //$scope.alertMessage = (date.title + ' was clicked ');
            swal({
                title: date.title,
                text: "Day Event!",
                imageUrl: 'https://jshsstorage.blob.core.windows.net/files/events-icon-4.jpg'
            });
        };
        /* alert on Drop */
        $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
            $scope.alertMessage = ('Event Dropped to make dayDelta ' + delta);
        };
        /* alert on Resize */
        $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
            $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
        };
        /* add and removes an event source of choice */
        $scope.addRemoveEventSource = function (sources, source) {
            var canAdd = 0;
            angular.forEach(sources, function (value, key) {
                if (sources[key] === source) {
                    sources.splice(key, 1);
                    canAdd = 1;
                }
            });
            if (canAdd === 0) {
                sources.push(source);
            }
        };
        /* add custom event*/
        $scope.addEvent = function () {
            $scope.events.push({
                title: $scope.ev.title,
                start: moment($scope.ev.from),
                //   end: moment($scope.ev.to),
                allDay: true,
                className: ['openSesame']
            });
        };
        /* remove event */
        /*$scope.remove = function (index) {
            $scope.events.splice(index, 1);
        };*/
        /* Change View */
        $scope.changeView = function (view, calendar) {
            uiCalendarConfig.calendars.myCalendar1.fullCalendar('removeEvents');
            uiCalendarConfig.calendars.myCalendar1.fullCalendar('addEventSource',
                $scope.tempcldrlst);
        };
        /* Change View */
        $scope.renderCalender = function (calendar) {
            $timeout(function () {
                if (uiCalendarConfig.calendars[calendar]) {
                    uiCalendarConfig.calendars[calendar].fullCalendar('render');
                }
            });
        };
        /* Render Tooltip */
        $scope.eventRender = function (event, element, view) { };
        /* config object */
        $scope.uiConfig = {
            calendar: {
                height: 325,

                editable: false,
                viewRender: $scope.changeView,
                //customButtons: {
                //    myCustomButton: {
                //        text: 'custom!',
                //        click: function () {
                //            alert('clicked the custom button!');
                //        }
                //    }
                //},
                header: {
                    left: 'title',

                    // center: 'myCustomButton',

                    //center: 'myCustomButton',

                    right: 'today prev,next'
                },
                dayClick: $scope.alertOnDayClick,
                eventClick: $scope.alertOnEventClick,
                eventDrop: $scope.alertOnDrop,
                eventResize: $scope.alertOnResize,
                eventRender: $scope.eventRender,
                businessHours: {
                    start: '12:00', // a start time (10am in this example)
                    //     end: '18:00', // an end time (6pm in this example)

                    dow: [1, 2, 3, 4]
                    // days of week. an array of zero-based day of week integers (0=Sunday)
                    // (Monday-Thursday in this example)
                }
            }
        };
        /* event sources array*/
        $scope.eventSources = [$scope.events, $scope.eventsF];
        $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];


        $scope.eventRender = function (event, element, view) {
            element.attr({
                'tooltip': event.events,
                'tooltip-append-to-body': true
            });
            $compile(element)($scope);
        };
        // #endregion

        var HostName = location.host;
        //Redirect to Fee 
        $scope.onfee = function () {
            $window.location.href = 'http://' + HostName + '/#/app/FeeDetails';
            $('.modal-backdrop').remove();
        };
        $scope.oncoe = function () {
            $window.location.href = 'http://' + HostName + '/#/app/COE';
            $('.modal-backdrop').remove();
        };

        $scope.saveakpkfile = function () {
            var data = {
                "MI_Id": $scope.mI_Id
            };
            apiService.create("StudentDashboard/saveakpkfile", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal('File Download!');
                    }
                    //else {
                    //    swal('File Not Saved!');
                    //}
                });
        };

        $scope.onAttendance = function () {
            $window.location.href = 'http://' + HostName + '/#/app/AttendanceDetails';
            $('.modal-backdrop').remove();
        };


        $scope.getmodeldetails = function (option) {
            $scope.allimages = [];
            if (option.ihW_FilePath == undefined || option.ihW_FilePath == null || option.ihW_FilePath == '') {
                var img = option.icW_FilePath;
            }
            else {
                var img = option.ihW_FilePath;
            }

            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];

                $scope.filetype2 = lastelement;
            }

            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.videofile = true;
                $scope.imgfile = false;
                $scope.allimages.push({ ihW_FilePath: img })
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'pdf') {

                $scope.allimages.push({ ihW_FilePath: img })
                $scope.imgfile = true;
                $scope.videofile = false;
            }
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        //=================== list doc==========
        $scope.viewData_doc_homework = function (work) {
            $scope.attachementlist1 = [];
            var data = {
                "IHW_Id": work.ihW_Id,
                "ASMAY_Id": work.asmaY_Id,
            };
            apiService.create("StudentDashboard/viewData", data).then(function (promise) {
                $scope.attachementlist = [];
                if (promise.attachementlist.length > 0) {
                    $scope.attachementlist1 = [];
                    angular.forEach(promise.attachementlist, function (qq) {
                        $scope.img = qq.ihwatT_Attachment;
                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];

                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                            $scope.attachementlist1.push({
                                ihwatT_FileName: qq.ihwatT_FileName,
                                ihW_Attachment: qq.ihW_Attachment,
                                ihW_Id: qq.ihW_Id, ihwatT_Attachment: qq.ihwatT_Attachment
                            })
                        }
                        else {
                            $scope.attachementlist1.push({
                                ihwatT_FileName: qq.ihwatT_FileName,
                                ihW_Attachment: 'HyperLink',
                                ihW_Id: qq.ihW_Id, ihwatT_Attachment: qq.ihwatT_Attachment
                            })
                        }
                    });
                    $scope.attachementlist = $scope.attachementlist1;
                    $('#myModalCoverview_home').modal('show');
                }
                else {
                    swal("No Data Found.")
                }
            });
        };

        $scope.viewData_doc_COE = function (work) {
            var data = {
                "COEE_Id": work.coeE_Id,
                "ASMAY_Id": work.asmaY_Id,
            };
            apiService.create("StudentDashboard/viewData", data).then(function (promise) {
                $scope.attachementlist1 = promise.attachementlist;
                $('#myModalCoverview_COE').modal('show');
            });
        };


        $scope.viewData_doc_class = function (work) {
            var data = {
                "ICW_Id": work.icW_Id,
                "ASMAY_Id": work.asmaY_Id
            };
            apiService.create("StudentDashboard/viewData", data).then(function (promise) {

                if (promise.attachementlist1.length > 0) {
                    $scope.attachementlist12 = [];
                    angular.forEach(promise.attachementlist1, function (qq) {
                        $scope.img = qq.icwatT_Attachment;

                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                            $scope.attachementlist12.push({
                                icwatT_FileName: qq.icwatT_FileName, icW_Attachment: qq.icW_Attachment,
                                icwatT_Attachment: qq.icwatT_Attachment
                            });
                        }
                        else {
                            $scope.attachementlist12.push({
                                icwatT_FileName: qq.icwatT_FileName, icW_Attachment: 'HyperLink',
                                icwatT_Attachment: qq.icwatT_Attachment
                            });
                        }
                    })

                    $scope.attachementlist1 = $scope.attachementlist12;
                    $('#myModalCoverview_class').modal('show');
                }
                else {
                    swal("No Data Found.")
                }
            });
        };

        //=====================view doc
        $scope.previewimg_new = function (img) {
            $('#myvideoPreview').modal('hide');
            $('#myimagePreview').modal('hide');
            $('#showpdf').modal('hide');
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'mp3') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)

            }

            else if ($scope.filetype2 == 'pdf') {
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;

                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                        document.getElementById("pdfIdzz").innerHTML = htmlElements;

                        //pdfId = document.getElementById("pdfIdzz");
                        //pdfId.removeChild(pdfId.childNodes[0]);
                        //embed = document.createElement('embed');
                        //embed.setAttribute('src', fileURL);
                        //embed.setAttribute('type', 'application/pdf');
                        //embed.setAttribute('width', '100%');
                        //embed.setAttribute('height', '1000');
                        //pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }

        };
        //================================ Homework Upload
        $scope.videoUpload = function (input, document) {
            $scope.ldr = true;
            $scope.hide = false;

            $scope.files = input.files;
            if (input.files && input.files[0]) {
                // if (input.files[0].type === "video/mp4" || input.files[0].type === "video/x-ms-wmv") {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadImgs();


                // }
                //else if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/pptx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                //    $scope.stepsModel = [];
                //    $scope.files = input.files;
                //    for (var i = 0; i < $scope.files.length; i++) {
                //        var file = $scope.files[i];
                //        $scope.fileimg = file;
                //        var reader = new FileReader();
                //        reader.onload = $scope.imageIsLoaded;
                //        reader.readAsDataURL(file);
                //        UploadImgs();
                //    }

                //    $scope.imageIsLoaded = function (e) {
                //        $scope.$apply(function () {
                //            $scope.stepsModel.push(e.target.result);
                //        });
                //    };
                //}

            }
        };
        function UploadImgs() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                $scope.filenames = "Videos";
                $scope.fileflg = true;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $scope.notice = [];
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.imagecount = 0;
                    $scope.images_paths = d;
                    $scope.notice = $scope.images_paths;

                    if ($scope.notice.length > 0) {
                        $scope.checkcount = true;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        //================================ Classwork Upload
        $scope.videoUpload1 = function (input, document) {
            $scope.ldr = true;
            $scope.hide = false;

            $scope.files = input.files;
            if (input.files && input.files[0]) {
                //if (input.files[0].type === "video/mp4" || input.files[0].type === "video/x-ms-wmv") {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadImgs1();


                //}
                //else if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/pptx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                //    $scope.stepsModel = [];
                //    $scope.files = input.files;
                //    for (var i = 0; i < $scope.files.length; i++) {
                //        var file = $scope.files[i];
                //        $scope.fileimg = file;
                //        var reader = new FileReader();
                //        reader.onload = $scope.imageIsLoaded;
                //        reader.readAsDataURL(file);
                //        UploadImgs1();
                //    }

                //    $scope.imageIsLoaded = function (e) {
                //        $scope.$apply(function () {
                //            $scope.stepsModel.push(e.target.result);
                //        });
                //    };
                //}

            }
        };
        function UploadImgs1() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                $scope.filenames = "Videos";
                $scope.fileflg = true;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $scope.notice = [];
            $http.post("/api/ImageUpload/ClassworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.imagecount = 0;
                    $scope.images_paths = d;
                    $scope.notice = $scope.images_paths;

                    if ($scope.notice.length > 0) {
                        $scope.checkcount = true;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }
        //================save class work doc=======

        $scope.savecls_dco = function (user1) {
            $scope.notice1 = [];
            angular.forEach($scope.documentListOtherDetails, function (aa) {
                if (aa.ICW_FilePath !== undefined && aa.ICW_FilePath !== null && aa.ICW_FilePath !== "") {
                    $scope.notice1.push({ DCO_Attachment: aa.ICW_FilePath, Doc_FileName: aa.FileName });
                }
            });

            if ($scope.notice1.length > 0) {
                var data = {
                    "AMST_Id": $scope.AMST_Id,
                    "ICW_Id": $scope.icW_Idnew,
                    "uploaddoc_array": $scope.notice1
                }
                apiService.create("StudentDashboard/savecls_doc", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Document Uploaded successfully');
                        $('#myModalmultipledoc').modal('hide');
                        $scope.loadclassworkdata();
                    }
                    else if (promise.returnval == false) {
                        swal('Document Not Upload successfully!!!');
                        $('#myModalmultipledoc').modal('show');
                        $scope.loadclassworkdata();
                    }
                })
            } else {
                swal("Kindly Upload File To Submit The Class Work");
            }
        }

        //================save class work doc=======

        $scope.savehome_dco = function () {

            $scope.notice1 = [];

            angular.forEach($scope.documentListOtherDetails, function (aa) {
                if (aa.IHW_FilePath !== undefined && aa.IHW_FilePath !== null && aa.IHW_FilePath !== "") {
                    $scope.notice1.push({ DCO_Attachment: aa.IHW_FilePath, Doc_FileName: aa.FileName })
                }
            });
            if ($scope.notice1.length > 0) {
                var data = {
                    "AMST_Id": $scope.AMST_Id,
                    "IHW_Id": $scope.ihwidnew,
                    "uploaddoc_array": $scope.notice1
                }
                apiService.create("StudentDashboard/savehome_doc", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Document Uploaded successfully');
                        $('#myModalmultipledoc').modal('hide');
                        $scope.loadhomeworkdata();
                    }
                    else if (promise.returnval == false) {
                        swal('Document Not Upload successfully!!!');
                        $('#myModalmultipledoc').modal('hide');
                        $scope.loadhomeworkdata();
                    }
                });
            } else {
                swal("Kindly Upload File To Submit The Home Work");
            }
        };

        $scope.optionToggled = function (user) {
            angular.forEach($scope.homeworklist, function (qq) {
                qq.checkedvalue = false;
            })
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.optionToggled1234 = function (user) {
            $scope.listdata = [];
            var count = 0;
            angular.forEach($scope.homeworklist, function (user) {
                if (user.checkedvalue == true) {
                    $scope.listdata.push(user);
                }
            })
            angular.forEach($scope.homeworklist, function (user) {
                user.checkedvalue = false;
            })
            angular.forEach($scope.homeworklist, function (dd) {
                angular.forEach($scope.listdata, function (ww) {
                    if (dd.ihW_Id == ww.ihW_Id) {
                        user.checkedvalue = true;
                        $scope.ihwidnew = dd.ihW_Id;
                        $scope.checkedvaluenew = true;
                    }
                });
            });
            if (user.checkedvalue == true) {

                $scope.checkcount = false;
                $scope.documentListOtherDetails = [{ id: 'document' }];
                $scope.addNewDocumentOtherDetail = function () {
                    var newItemNo = $scope.documentListOtherDetails.length + 1;
                    if (newItemNo <= 30) {
                        $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
                    }
                };
                $('#myModalmultipledoc').modal('show');
            }
        };

        $scope.optionToggledclasswork = function (user) {
            $scope.icwupLMarks = user.icwupL_Marks;
            $scope.icW_Idnew = user.icW_Id;
            $scope.listdata = [];
            var count = 0;
            angular.forEach($scope.assignmentarray, function (user) {
                if (user.checkedvalue == true) {
                    $scope.listdata.push(user);
                }
            })
            angular.forEach($scope.assignmentarray, function (user) {
                user.checkedvalue = false;
            })

            angular.forEach($scope.assignmentarray, function (dd) {
                angular.forEach($scope.listdata, function (ww) {
                    if (dd.icW_Id == ww.icW_Id) {
                        user.checkedvalue = true;
                    }
                });
            });


            if (user.checkedvalue == true) {
                $scope.checkcount = false;

                $scope.documentListOtherDetails = [{ id: 'document' }];
                $scope.addNewDocumentOtherDetail = function () {
                    var newItemNo = $scope.documentListOtherDetails.length + 1;
                    if (newItemNo <= 30) {
                        $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
                    }
                };
                $('#myModalmultipledoc').modal('show');
            }
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.homeworklist, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }

        $scope.optionToggled1 = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })

        }

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.assignmentarray, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }

        //========================= view doc================
        $scope.viewData_upDoc_homework = function (work) {
            $scope.attachementlist1 = [];
            $scope.ihwupLMarks = work.ihwupL_Marks;
            $scope.ihwidnew = work.ihW_Id;


            var data = {
                "IHW_Id": work.ihW_Id,
                "ASMAY_Id": work.asmaY_Id
            };
            apiService.create("StudentDashboard/viewData_doc", data).then(function (promise) {
                $scope.attachementlist2 = [];
                $scope.documentListOtherDetails = [];

                $scope.attachementlist2 = promise.attachementlist;
                if ($scope.attachementlist2 != null && $scope.attachementlist2.length > 0) {
                    angular.forEach($scope.attachementlist2, function (aa) {
                        $scope.documentListOtherDetails.push({
                            id: 1, IHW_FilePath: aa.ihwupL_Attachment,
                            FileName: aa.ihwupL_FileName
                        });
                    });
                }

                if ($scope.documentListOtherDetails != null && $scope.documentListOtherDetails.length > 0) {
                    //$scope.documentListOtherDetails = [{ id: 'document' }];
                    $scope.addNewDocumentOtherDetail = function () {
                        var newItemNo = $scope.documentListOtherDetails.length + 1;
                        if (newItemNo <= 30) {
                            $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
                        }
                    };
                }

                $('#myModalmultipledoc').modal('show');
            });
        };

        $scope.viewData_upDoc_classwork = function (work) {
            $scope.attachementlist1 = [];
            $scope.icwupLMarks = work.icwupL_Marks;
            $scope.icW_Idnew = work.icW_Id;

            var data = {
                "ICW_Id": work.icW_Id,
                "ASMAY_Id": work.asmaY_Id
            };
            apiService.create("StudentDashboard/viewData_doc", data).then(function (promise) {
                $scope.attachementlist3 = [];
                $scope.documentListOtherDetails = [];

                $scope.attachementlist3 = promise.attachementlist1;
                angular.forEach($scope.attachementlist3, function (aa) {
                    $scope.documentListOtherDetails.push({
                        id: 1, ICW_FilePath: aa.icwupL_Attachment,
                        FileName: aa.icwupL_FileName, UploadedFlag: 1
                    });
                });
                if ($scope.documentListOtherDetails.length == 0) {
                    $scope.documentListOtherDetails = [{ id: 'document', UploadedFlag: 0 }];

                    $scope.addNewDocumentOtherDetail = function () {
                        var newItemNo = $scope.documentListOtherDetails.length + 1;
                        if (newItemNo <= 30) {
                            $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo, UploadedFlag: 0 });
                        }
                    };
                }
                $('#myModalmultipledoc').modal('show');
            });

        };
        $scope.uploadHW = function () {
            $window.location.href = 'http://' + HostName + '/#/app/Student_Homework';
            $('#myModalhomeworkd1').modal('hide');
            $('.modal-backdrop').remove();
        };
        $scope.uploadCH = function () {
            $window.location.href = 'http://' + HostName + '/#/app/Student_Classwork';
            $('.modal-backdrop').remove();
        };

        //==============view data=========

        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "INTB_Id": option.intB_Id

            };
            apiService.create("StudentDashboard/viewnotice", data).then(function (promise) {
                if (promise.attachementlist.length > 0) {
                    //    $scope.attachementlist1 = [];
                    //    angular.forEach(promise.attachementlist, function (qq) {
                    //        for (i = 0; i < $scope.attachementlist.length; i++) {

                    //            if ($scope.attachementlist[i].intbfL_FilePath !== "") {

                    //        $scope.img = qq.intbfL_FilePath;
                    //        if ($scope.img != null) {
                    //            var imagarr = $scope.img.split('.');
                    //            var lastelement = imagarr[imagarr.length - 1];

                    //            $scope.filetype2 = lastelement;
                    //        }

                    //        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                    //            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                    //            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                    //            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                    //            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                    //            $scope.attachementlist1.push({
                    //                INTBFL_FileName: qq.intbfL_FileName,
                    //                INTBFL_FilePath: qq.intbfL_FilePath,
                    //                INTB_Attachment: qq.intB_Attachment,
                    //                INTB_Id: qq.intB_Id
                    //            })
                    //        }
                    //        else {
                    //            $scope.attachementlist1.push({

                    //                INTBFL_FileName: qq.intbfL_FileName,
                    //                INTBFL_FilePath: qq.intbfL_FilePath,
                    //                INTB_Attachment: 'HyperLink',
                    //                INTB_Id: qq.intB_Id

                    //            })
                    //        }
                    //    })

                    //    $scope.attachementlist = $scope.attachementlist1;

                    //    $('#myModalCoverview').modal('show');
                    //    $scope.docshowary = true;
                    //    $scope.docshow = false;
                    //}
                    //else {
                    //    swal("No Data Found.")

                    //}





                    $scope.attachementlist = promise.attachementlist;
                    $scope.attachementlist1 = [];
                    //angular.forEach(promise.attachementlist, function (qq1) {
                   // for (i = 0; i < $scope.attachementlist.length; i++) {
                        //$scope.attachementlist[i].intbfL_FilePath !== null || && $scope.attachementlist[i].intB_Attachment !== null || $scope.attachementlist[i].intB_Attachment !== ""
                      //  if ($scope.attachementlist[i].intbfL_FilePath !== "") {

                            angular.forEach(promise.attachementlist, function (qq) {
                                $scope.img = qq.intbfL_FilePath;
                                if ($scope.img != null) {
                                    var imagarr = $scope.img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    $scope.filetype2 = lastelement;
                                }

                                if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                    || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                    $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                    || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                    || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                    $scope.attachementlist1.push({
                                        INTBFL_FileName: qq.intbfL_FileName,
                                        INTBFL_FilePath: qq.intbfL_FilePath,
                                        INTB_Attachment: qq.intB_Attachment,
                                        INTB_Id: qq.intB_Id
                                    })
                                }
                                else {
                                    $scope.attachementlist1.push({

                                        INTBFL_FileName: qq.intbfL_FileName,
                                        INTBFL_FilePath: qq.intbfL_FilePath,
                                        INTB_Attachment: 'HyperLink',
                                        INTB_Id: qq.intB_Id

                                    })
                                }
                            })

                            $scope.attachementlist = $scope.attachementlist1;

                            $('#myModalCoverview').modal('show');
                            $scope.docshowary = true;
                            $scope.docshow = false;

                        //} else {

                        //    swal("No Data Found.")
                        //}
                  //  }
                }
                else {
                    swal("No Data Found.")

                }

            });


        };


        //===================== end==============================
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        //===================== multiple add + - Start ==================
        $scope.documentListOtherDetails = [{ id: 'document', UploadedFlag: 0 }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo, UploadedFlag: 0 });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };

        $scope.OnClickHomePage = function () {
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/StudentDashboard';
        };


        //===================== multiple add + - End ==================
        //============================== File Upload Start=========
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD').attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };

        $scope.selectFileforUploadzdOtherDetailhome = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };

        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.IHW_FilePath = d[0].path;
                        data.FileName = d[0].name;
                        $scope.ldr = false;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.interacted13 = function (field) {
            return $scope.submitted13;
        };
        //============================== File Upload End=========
        //=====================add mul class start========================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //=====================add mul class End========================

        //============= class upload doc start=============

        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                document.extention = nameArray[nameArray.length - 1];
                if (extention === "doc" || extention === "xlsx" || extention === "jpg" || extention === "jpeg" ||
                    extention === "xls" || extention === "png"
                    || extention === "pdf"
                    || extention === "pptx" || extention === "ppsx" || extention === "ppt"
                    || extention === "mp3"
                    || extention === "mp4"
                    || extention === "docx" || extention === "wmv") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentOtherDetail1(document);
                }

            }
        };

        $scope.selectFileforUploadzdOtherDetailClass = function (input, document) {
            $scope.ldr = true;
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                document.extention = nameArray[nameArray.length - 1];
                if (extention === "doc" || extention === "xlsx" || extention === "jpg" || extention === "jpeg" ||
                    extention === "xls" || extention === "png" || extention === "pdf" || extention === "pptx" || extention === "ppsx"
                    || extention === "ppt" || extention === "mp3" || extention === "mp4" || extention === "docx" || extention === "wmv") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentOtherDetail1(document);
                }
            }
        };

        function UploadEmployeeDocumentOtherDetail1(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/ClassworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.ICW_FilePath = d[0].path;
                        data.FileName = d[0].name;
                        $scope.ldr = false;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }


        // View Student Profile

        $scope.ViewStudentProfile = function () {
            $scope.myTabIndex2 = 0;
            $scope.year_name = "";
            $scope.viewstudentexamsubjectdetails = [];
            $scope.viewstudentwiseexamdetails = [];
            $scope.viewstudentattendancetails = [];
            $scope.viewstudentsubjectdetails = [];
            $scope.viewstudentfeedetails = [];
            $scope.studentdivlist = [];
            $scope.btnshow = false;
            $scope.showfee = true;
            var data = {
                "AMST_Id": $scope.AMST_Id_session,
                "student_staffflag": 'Student'
            };

            apiService.create("StudentDashboard/ViewStudentProfile", data).then(function (promise) {

                if (promise !== null) {
                    $scope.viewstudentjoineddetails = promise.viewstudentjoineddetails;

                    if ($scope.viewstudentjoineddetails !== null && $scope.viewstudentjoineddetails.length > 0) {
                        $scope.studentname_view = $scope.viewstudentjoineddetails[0].studentname;
                        $scope.amstadmno_view = $scope.viewstudentjoineddetails[0].amsT_AdmNo;
                        $scope.amstregno_view = $scope.viewstudentjoineddetails[0].amsT_RegistrationNo;
                        $scope.year_view = $scope.viewstudentjoineddetails[0].asmaY_Year;
                        $scope.class_view = $scope.viewstudentjoineddetails[0].asmcL_ClassName;
                        $scope.photo_view = $scope.viewstudentjoineddetails[0].amsT_Photoname;
                        $scope.gender_view = $scope.viewstudentjoineddetails[0].amsT_Sex;
                        $scope.status_view = $scope.viewstudentjoineddetails[0].amsT_SOL;
                        $scope.doa_view = new Date($scope.viewstudentjoineddetails[0].amsT_Date);
                        $scope.dob_view = new Date($scope.viewstudentjoineddetails[0].amsT_DOB);

                        $scope.viewstudentdetails = promise.viewstudentdetails;

                        if ($scope.viewstudentdetails !== null && $scope.viewstudentdetails.length > 0) {
                            $scope.mobile_view = $scope.viewstudentdetails[0].amsT_MobileNo;
                            $scope.email_view = $scope.viewstudentdetails[0].amsT_emailId;
                            $scope.stutpin = $scope.viewstudentdetails[0].amsT_Tpin;
                            //Father Details
                            $scope.FatherName = $scope.viewstudentdetails[0].amsT_FatherName;
                            $scope.FatherSurName = $scope.viewstudentdetails[0].amsT_FatherSurname === null
                                || $scope.viewstudentdetails[0].amsT_FatherSurname === "" ? "" : $scope.viewstudentdetails[0].amsT_FatherSurname;
                            $scope.Father_MobileNo = $scope.viewstudentdetails[0].amsT_FatherMobleNo;
                            $scope.Father_EmailId = $scope.viewstudentdetails[0].amsT_FatheremailId;
                            $scope.Father_photo = $scope.viewstudentdetails[0].ansT_FatherPhoto;


                            //Mother Details
                            $scope.MotherName = $scope.viewstudentdetails[0].amsT_MotherName;
                            $scope.MotherSurName = $scope.viewstudentdetails[0].amsT_MotherSurname === null || $scope.viewstudentdetails[0].amsT_MotherSurname === "" || $scope.viewstudentdetails[0].amsT_MotherSurname === "0" ? "" : ' ' + $scope.viewstudentdetails[0].amsT_MotherSurname;
                            $scope.Mother_MobileNo = $scope.viewstudentdetails[0].amsT_MotherMobileNo;
                            $scope.Mother_EmailId = $scope.viewstudentdetails[0].amsT_MotherEmailId;
                            $scope.Mother_photo = $scope.viewstudentdetails[0].ansT_MotherPhoto;

                        }

                        if (promise.viewstudentacademicyeardetails !== null && promise.viewstudentacademicyeardetails.length > 0) {
                            $scope.viewstudentacademicyeardetails = promise.viewstudentacademicyeardetails;
                        }

                        if (promise.viewstudentguardiandetails !== null && promise.viewstudentguardiandetails.length > 0) {
                            $scope.viewstudentguardiandetails = promise.viewstudentguardiandetails;
                        }

                        //Over All Attendance Details
                        $scope.att_workingdays = [];
                        $scope.att_presentdays = [];
                        $scope.att_percentage = [];
                        if (promise.viewstudentattendancetails !== null && promise.viewstudentattendancetails.length > 0) {
                            $scope.viewstudentattendancetails = promise.viewstudentattendancetails;

                            angular.forEach($scope.viewstudentattendancetails, function (d) {
                                $scope.att_workingdays.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.WORKINGDAYS });
                                $scope.att_presentdays.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.PRESENTDAYS });
                                $scope.att_percentage.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.PERCENTAGE });
                            });

                            var chart = new CanvasJS.Chart("att_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Working Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_workingdays
                                },
                                {
                                    name: "Present Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_presentdays
                                },
                                {
                                    name: "Percentage",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_percentage
                                }]
                            });
                            chart.render();
                        }

                        // Year Month Wise Attendance Details
                        $scope.viewstudentattendanceMonthdetails = [];
                        $scope.att_Month_workingdays = [];
                        $scope.att_Month_presentdays = [];
                        if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                            $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;

                            angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                                $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                                $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                            });

                            var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Working Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_workingdays
                                },
                                {
                                    name: "Present Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_presentdays
                                }]
                            });
                            chart.render();
                        }


                        //Subject Details For Current Year
                        if (promise.viewstudentsubjectdetails !== null && promise.viewstudentsubjectdetails.length > 0) {
                            $scope.viewstudentsubjectdetails = promise.viewstudentsubjectdetails;
                        }

                        //Exam Details
                        if (promise.viewstudentwiseexamdetails !== null && promise.viewstudentwiseexamdetails.length > 0) {
                            $scope.viewstudentwiseexamdetails = promise.viewstudentwiseexamdetails;
                        }


                        // Fee Details
                        $scope.fee_yearlycharges = [];
                        $scope.fee_Concession = [];
                        $scope.fee_Payable = [];
                        $scope.fee_PaidAmount = [];
                        $scope.fee_Outstanding = [];
                        $scope.class_feeminus = "fa-minus";
                        if (promise.viewstudentfeedetails !== null && promise.viewstudentfeedetails.length > 0) {
                            $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                            angular.forEach($scope.viewstudentfeedetails, function (d) {
                                $scope.fee_yearlycharges.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.YearlyCharges });
                                $scope.fee_Concession.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Concession });
                                $scope.fee_Payable.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Payable });
                                $scope.fee_PaidAmount.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.PaidAmount });
                                $scope.fee_Outstanding.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Outstanding });
                            });

                            var chart = new CanvasJS.Chart("fee_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Yearly Charges",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_yearlycharges
                                },
                                {
                                    name: "Concession",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Concession
                                },
                                {
                                    name: "Payable",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Payable
                                },
                                {
                                    name: "Paid Amount",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_PaidAmount
                                },
                                {
                                    name: "Outstanding",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Outstanding
                                }]
                            });
                            chart.render();
                        }

                        // Fee Yearly Paid Details
                        $scope.viewstudenfeeyeardetails = [];
                        if (promise.viewstudenfeeyeardetails !== null && promise.viewstudenfeeyeardetails.length > 0) {
                            $scope.viewstudenfeeyeardetails = promise.viewstudenfeeyeardetails;
                        }

                        //Compliants list
                        if (promise.studentdivlist !== null && promise.studentdivlist.length > 0) {
                            $scope.studentdivlist = promise.studentdivlist;

                            angular.forEach($scope.studentdivlist, function (dd) {
                                if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                                    var img = dd.ASCOMP_FilePath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("data.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                                    }
                                }
                            });
                        }

                        //Address
                        if (promise.viewstudentaddressdetails !== null && promise.viewstudentaddressdetails.length > 0) {
                            $scope.viewstudentaddressdetails = promise.viewstudentaddressdetails;

                            if ($scope.viewstudentaddressdetails[0].PermanentAddress !== null && $scope.viewstudentaddressdetails[0].PermanentAddress !== "") {
                                $scope.permanentaddress = $scope.viewstudentaddressdetails[0].PermanentAddress.split(',');
                            }

                            if ($scope.viewstudentaddressdetails[0].ContactAddress !== null && $scope.viewstudentaddressdetails[0].ContactAddress !== "") {
                                $scope.communicationaddress = $scope.viewstudentaddressdetails[0].ContactAddress.split(',');
                            }
                        }

                        //staff list
                        if (promise.classteacher !== null && promise.classteacher.length > 0) {
                            $scope.classteacher = promise.classteacher;
                        }

                        if (promise.sujectteachers !== null && promise.sujectteachers.length > 0) {
                            $scope.sujectteachers = promise.sujectteachers;
                        }



                        $('#mymodalviewdetais').modal('show');
                    }
                }
            });
        };

        $scope.ViewMonthWiseAttendance = function (dd) {

            $scope.viewstudentattendanceMonthdetails = [];
            $scope.att_Month_workingdays = [];
            $scope.att_Month_presentdays = [];
            $scope.year_name_att = dd.ASMAY_Year;
            document.getElementById('att_Month_profile_chartContainer').innerHTML = "";
            var data = {
                "AMST_Id": $scope.AMST_Id_session,
                "ASMAY_Id": dd.ASMAY_Id,
                "student_staffflag": 'Student'
            };

            apiService.create("StudentDashboard/ViewMonthWiseAttendance", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                        $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;
                        angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                            $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                            $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                        });

                        var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 13,
                            },
                            axisY: {
                                labelFontSize: 13,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                name: "Working Days",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.att_Month_workingdays
                            },
                            {
                                name: "Present Days",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.att_Month_presentdays
                            }]
                        });
                        chart.render();

                        $('#mymodalviewdetais').animate({ scrollTop: 500 }, 'slow');
                    }
                }
            });
        };


        $scope.ViewYearWiseFee = function (dd) {
            $scope.viewstudenfeeyeardetails = [];
            $scope.year_name_fee = dd.asmay_year;
            var data = {
                "AMST_Id": $scope.AMST_Id_session,
                "ASMAY_Id": dd.ASMAY_Id,
                "student_staffflag": 'Student'
            };

            apiService.create("StudentDashboard/ViewYearWiseFee", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewstudenfeeyeardetails !== null && promise.viewstudenfeeyeardetails.length > 0) {
                        $scope.showfee = false;
                        $scope.btnshow = false;
                        $scope.viewstudenfeeyeardetails = promise.viewstudenfeeyeardetails;
                        $('#mymodalviewdetais').animate({ scrollTop: 500 }, 'slow');
                    }
                }
            });
        };

        $scope.ViewExamSubjectWiseDetails = function (dd) {
            $scope.viewstudentexamsubjectdetails = [];
            $scope.examname = dd.EME_ExamName;
            var data = {
                "EME_Id": dd.EME_Id,
                "ASMAY_Id": dd.ASMAY_Id,
                "AMST_Id": $scope.AMST_Id_session,
                "student_staffflag": 'Student'
            };

            apiService.create("StudentDashboard/ViewExamSubjectWiseDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewstudentexamsubjectdetails = promise.viewstudentexamsubjectdetails;
                }
            });
        };



        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf1').modal('show');
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.onclick_booklist = function (flag) {
            $scope.booklist = [];
            $('#myModalbooklist').modal('hide');
            var data = {
                "flag": "BOOK"
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.booklist = promise.assetlist;
                    $('#myModalbooklist').modal('show');

                }
                else {
                    swal('No Data Found..!!');

                }

            }
            );
        };

        $scope.onclick_booklistwo = function (flag) {
            $scope.bookFillist = [];

            var data = {
                "flag": "BOOKLIST",
                "INTB_Id": $scope.LMB_Id.LMB_Id
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.bookFillist = promise.assetlist;
                }
                else if (promise.displyamessage.length > 0) {
                    $scope.displyamessages = $scope.displyamessage
                }

                else {
                    swal('No Data Found..!!');

                }

            }
            );
        };


        $scope.downloadAll = function () {
            var downloadLinks = [];

            angular.forEach($scope.imagegallery, function (ig) {

                if (ig.IGAP_Id != null) {
                    var imageLink = document.createElement('a');
                    imageLink.href = ig.IGAP_Photos;
                    imageLink.download = 'image_' + ig.IGAP_Id + '.jpg'; 
                    downloadLinks.push(imageLink);
                }


                if (ig.IGAV_Id != null) {
                    var videoLink = document.createElement('a');
                    videoLink.href = ig.IGAV_Videos;
                    videoLink.download = 'video_' + ig.IGAV_Id + '.mp4'; 
                    downloadLinks.push(videoLink);
                }
            });


            function triggerDownloads() {
                if (downloadLinks.length === 0) return;

                var link = downloadLinks.shift(); 
                link.style.display = 'none'; 
                document.body.appendChild(link); 
                link.click(); 
                document.body.removeChild(link); 
                setTimeout(triggerDownloads, 100); 
            }

            triggerDownloads();
        };






        //===================
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });


})();
