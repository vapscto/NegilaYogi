(function () {
    'use strict';
    angular
.module('app')
        .controller('DOBcertificateController', DOBcertificateController)

    DOBcertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function DOBcertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.print_flag = true;
        $scope.grid_flag = false;
        //$scope.pagesrecord = {};
        $scope.stud_name_flag = false;
        $scope.objj = {};
        $scope.onpageload = function ()
        {
            var pageid = 1;
            apiService.getURI("DOBcertificate/getdata", pageid).
        then(function (promise) {
            $scope.yearlist = promise.allAcademicYear;
            $scope.classlist = promise.allClass;
            $scope.sectionlist = promise.allSection;
            $scope.studlist = promise.adm_m_student;
            $scope.categoryDropdown = promise.category_list;

            $scope.categoryflag = promise.categoryflag;
        })
        }

       

        $scope.order = function (keyname)
        {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //Get Student by class
        $scope.stud_name_flag = true;

        $scope.onSelectGetStudent = function ()
        {
            
            var acedamicId = $scope.asmaY_Id;
            var sectionId = $scope.asmC_Id;
            var classId = $scope.asmcL_Id1;

            if ($scope.asmaY_Id != undefined && $scope.asmcL_Id1 == undefined && $scope.asmC_Id == undefined)
            {
                acedamicId = $scope.asmaY_Id;
                classId = "0";
                sectionId = "0";
                }
          

            else if ($scope.asmaY_Id != undefined && $scope.asmcL_Id1 != undefined && $scope.asmC_Id == undefined)
            {
                 acedamicId = $scope.asmaY_Id;
                 classId = $scope.asmcL_Id1;
                 sectionId = "0";
                
             }
            else
             {
                 acedamicId = $scope.asmaY_Id;
                 sectionId = $scope.asmC_Id;
                 classId = $scope.asmcL_Id1;
             }

                var data = {

                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                
                    }

            
             apiService.create("DOBcertificate/getstudbyclass", data).then(function (promise)
            {
                if (promise.fillstudlist != null && promise.fillstudlist.length > 0)
                {
                    $scope.studlist = promise.fillstudlist;
                    $scope.stud_name_flag = false;
                }
                else {
                    swal("Student is not there for this selection")
                    $scope.stud_name_flag = true;
                }
               
            })
        }




        $scope.submitted = false;
        $scope.Report = function ()
        {
            

            
            if ($scope.myForm.$valid)
            {
                var acedamicId = $scope.asmaY_Id;
                var sectionId = $scope.asmC_Id;
                var classId = $scope.asmcL_Id1;
                var studId = $scope.Student_Id;

                if (acedamicId == undefined || acedamicId == "")
                {
                    acedamicId = 0;
                }
                if (classId == undefined || classId == "")
                {
                    classId = 0;
                }
                if (sectionId == undefined || sectionId == "")
                {
                    sectionId = 0
                }

                var AMC_Id = 0
                if ($scope.objj.amC_Id != null && $scope.objj.amC_Id != 'All' && $scope.objj.amC_Id != undefined) {
                    AMC_Id = $scope.objj.amC_Id
                }
                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": studId,
                    "AMC_Id": AMC_Id
                }
                
               
                apiService.create("DOBcertificate/Studdetails", data)
                    .then(function (promise)
                    {
                        if (promise.studentlist != null && promise.studentlist.length > 0)
                        {
                            $scope.students = promise.studentlist;
                            $scope.print_flag = false;
                            $scope.grid_flag = true;
                            $scope.stunameF = $scope.Fname(promise.studentlist);
                            $scope.stunameM = $scope.Mname(promise.studentlist);
                            $scope.stunameL = $scope.Lname(promise.studentlist);
                            $scope.fatherNameC = $scope.fthrname(promise.studentlist);
                            $scope.classNameC = $scope.classname(promise.studentlist);
                            $scope.acyrC = $scope.acayyername(promise.studentlist);
                            
                            $scope.dobC =$scope.dateofbirth(promise.studentlist);
                            $scope.dobCW = $scope.dateofbirthWord(promise.studentlist);


                            if (promise.AMC_logo != null) {
                                $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                            }
                            else {
                               // $scope.imgname = logopath;
                            }

                                $scope.purpose = $scope.pur;
                               
                            
                            $('#studentpic').attr('src', $scope.stuPhoto(promise.stuPhoto));
                           
                            //$scope.stuPhoto = value.stuPhoto;
                        }
                        else {
                            swal("Record not found");
                            $scope.print_flag = true;
                            $scope.grid_flag = false;
                        }
                        //console.log(promise.studentlist);
                    })
            }
            else {
                $scope.submitted = true;
            }
            
        }

        $scope.Fname = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.stuFN; });
            return dobcerti;
        };
        $scope.Mname = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.stuMN; });
            return dobcerti;
        };
        $scope.Lname = function (int) {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.stuLN; });
            return dobcerti;
        };

        $scope.fthrname = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.fatherName; });
            return dobcerti;
        };
        $scope.classname = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.class; });
            return dobcerti;
        };
        $scope.acayyername = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.acadamicyear; });
            return dobcerti;
        };
        $scope.dateofbirth = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.dob; });
            return dobcerti;
        };
        $scope.dateofbirthWord = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.dobWord; });
            return dobcerti;
        };
        $scope.stuPhoto = function (int)
        {
            var dobcerti;
            angular.forEach($scope.students, function (e) { dobcerti = e.stuPhoto; });
            return dobcerti;
        };

        var studclear = [];
        $scope.Clearid = function ()
        {

           $state.reload();
            //$scope.asmaY_Id = "";
            //$scope.asmcL_Id1 = "";
            //$scope.asmC_Id = "";
            //$scope.Student_Id = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.print_flag = true;
            //$scope.grid_flag = false;
            //$scope.pur = "";
            //$scope.stud_name_flag = false;
           
           
        }

        
        $scope.CurrentDate = new Date();

        
        
        $scope.interacted = function (field)
        {

            return $scope.submitted;
        };

        //print
        //$scope.printToCart = function ()
        //{
        //    var innerContents = document.getElementById(printSectionId).innerHTML;
        //    var popupWinindow = window.open('', '_blank', 'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close("");
        //}



        $scope.printToCart = function ()
        {
            
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                 '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
            
        }
          
       

    }
})();

