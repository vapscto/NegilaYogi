﻿
(function () {
    'use strict';
    angular
        .module('app')
        .controller('NewChairmanDashboardController', NewChairmanDashboardController);
    NewChairmanDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings'];

    function NewChairmanDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings) {
        var miid = "";

        $scope.closeupdate = false;
        $scope.conformflag = true;

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });



       

        $scope.showtext = false;
        $scope.showtextdetails = "";      
        $scope.currentPage1 = 1;
        $scope.currentPage5 = 1;
        $scope.currentPage4 = 1;
        $scope.currentPageS = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage5 = 10;
        $scope.itemsPerPage4 = 10;
        $scope.itemsPerPageS = 10;

        

        $scope.loaddata = function () {

        $scope.showStudent = function () {
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.stdcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.stdcount.push(promise.getReport[i].studentcount);
                }
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }

        $scope.showEmployee = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.empcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.empcount.push(promise.getReport[i].Employeecount);
                }

        $scope.showPayment = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.amountcollected = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.amountcollected.push(promise.getReport[i].FSS_PaidAmount);
                }
                $scope.amounttobepaid = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.amounttobepaid.push(promise.getReport[i].Tobepaid);
                }
                $scope.amountchargable = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.amountchargable.push(promise.getReport[i].FSS_CurrentYrCharges);
                }


        $scope.showPreadmissions = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.precount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.precount.push(promise.getReport[i].Preadmissioncount);
                }

        $scope.showAdmissions = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.admcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.admcount.push(promise.getReport[i].admissioncount);
                }
                

                function createadmissionchart() {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }


        $scope.showtcIssued = function () {
            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.tcissucount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.tcissucount.push(promise.getReport[i].tccount);
                }
                function createtcchart() {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }

        $scope.showSalary = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.salcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.salcount.push(promise.getReport[i].salarycount);
                }
                function createsalarychart() {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }


        $scope.showPassFail = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.passcountS = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.passcountS.push(promise.getReport[i].PASS_PERCENTAGE);
                }
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.failcountS.push(promise.getReport[i].FAIL_PERCENTAGE);
                }


                function createresultchart() {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }

        $scope.showBooks = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.bokcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.bokcount.push(promise.getReport[i].bookcount);
                }

                function createbookchart() {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }
        $scope.showEvents = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.evecount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.evecount.push(promise.getReport[i].eventcount);
                }
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }
     

        $scope.showTransport = function () {


            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.transcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.transcount.push(promise.getReport[i].transport);
                }


        $scope.showDefaulters = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.defalcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.defalcount.push(promise.getReport[i].defaultercount);
                }


                function createdefaulterchart() {

    

        $scope.showAttendance = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.absentcounts = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.absentcounts.push(promise.getReport[i].Absent);
                }
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.Presentcounts.push(promise.getReport[i].present);
                }


                function createattendencechart() {

       


        $scope.showInteractions = function () {

            $scope.currentPage = 1;
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.intercount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.intercount.push(promise.getReport[i].interactioncount);
                }

    }
})();