// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


        // --- State Variables ---
        let currentStep = 1;
        let selectedRoom = null;
        let bookingType = null;
        let tenantData = {
            fullName: '',
            email: '',
            phone: '',
            idDocumentUrl: '',
        };
        let isSigned = false;

        // --- Mock Data ---
        const MOCK_ROOMS = [
            { id: 'A301', name: 'The Aspen Suite', status: 'Available', type: 'Studio', rent: 1800, details: 'Fully furnished, high-speed internet included.' },
            { id: 'B105', name: 'The Birch Residence', status: 'Available', type: '1 Bed', rent: 2500, details: 'In-unit laundry, corner unit with city view.' },
            { id: 'C402', name: 'The Cedar Loft', status: 'Occupied', type: '2 Bed', rent: 3200, details: 'Currently leased, available next quarter.' },
            { id: 'D208', name: 'The Dahlia Studio', status: 'Available', type: 'Studio', rent: 1750, details: 'Ground floor access, large patio.' },
        ];

        const getMockLeaseAgreement = (room, tenant) => {
            const date = new Date().toLocaleDateString();
            const rentAmount = room ? `R${room.rent}.00` : 'R0.00';
            const tenantName = tenant.fullName || '[Tenant Name]';
            const roomId = room ? room.id : '[Room ID]';
            const roomName = room ? room.name : '[Room Name]';

            return `


LEASE AGREEMENT SUMMARY
This agreement is entered into on ${date} between [Manager Name] (Landlord) and ${tenantName} (Tenant) for the property identified as ${roomId} - ${roomName}.

1. Term: 12 months, commencing on ${date}.
2. Rent: ${rentAmount}.
3. Security Deposit: ${rentAmount}.
4. Tenant Obligations: Maintain property, adhere to community rules.

By signing below, the Tenant acknowledges and agrees to the full terms and conditions of the Master Lease Agreement.
`;
        };

        // --- Utility Functions ---

        const flowSteps = [
            'Dashboard', 'Rooms List', 'Booking Choice', 'Lease Form', 'E-Sign', 'Payment', 'Confirmation'
        ];

        const setStep = (newStep) => {
            currentStep = newStep;
            window.scrollTo(0, 0); // Scroll to top on step change
            renderStep();
        };

        const Card = (title, content) => `
            <div class="bg-white p-6 shadow-xl rounded-xl w-full max-w-4xl mx-auto my-4 transition duration-300 hover:shadow-2xl">
                ${title ? `<h2 class="text-2xl font-semibold text-gray-800 mb-4 border-b pb-2">${title}</h2>` : ''}
                ${content}
            </div>
        `;

        const Button = (text, onClickFn, color = 'indigo', disabled = false, id = null) => {
            const disabledClass = 'bg-gray-400 cursor-not-allowed';
            const activeClass = `bg-${color}-600 hover:bg-${color}-700 shadow-md hover:shadow-lg focus:outline-none focus:ring-4 focus:ring-${color}-500 focus:ring-opacity-50`;
            const classes = `px-6 py-3 rounded-lg font-medium text-white transition duration-200 ${disabled ? disabledClass : activeClass}`;
            return `<button ${id ? `id="${id}"` : ''} onclick="${onClickFn}" class="${classes}" ${disabled ? 'disabled' : ''}>${text}</button>`;
        };

        const InfoBox = (label, value) => `
            <div class="flex justify-between items-center py-2 border-b last:border-b-0">
                <span class="text-gray-500 font-medium">${label}</span>
                <span class="text-gray-800 font-semibold">${value}</span>
            </div>
        `;

        // --- Step 1: Dashboard ---
        const renderDashboard = () => {
            const availableRooms = MOCK_ROOMS.filter(r => r.status === 'Available').length;
            const occupiedRooms = MOCK_ROOMS.length - availableRooms;

            const content = `
                <div class="grid grid-cols-1 md:grid-cols-3 gap-6 text-center">
                    <div class="p-5 bg-green-50 rounded-lg shadow-inner flex flex-col justify-center">
                        <p class="text-lg text-green-800 font-semibold mb-3">Start New Booking</p>
                        ${Button('View Available Rooms', 'setStep(2)')}
                    </div>
                </div>
            `;
            document.getElementById('content-area').innerHTML = Card("Manager Dashboard", content);
        };

        // --- Step 2: Available Rooms List ---
        const handleSelectRoom = (roomId) => {
            selectedRoom = MOCK_ROOMS.find(r => r.id === roomId);
            if (selectedRoom) {
                setStep(3);
            }
        };

        const renderRoomList = () => {
            const availableRooms = MOCK_ROOMS.filter(r => r.status === 'Available');

            let roomListHTML = availableRooms.length > 0 ?
                availableRooms.map((room) => `
                    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center p-4 bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md transition duration-150">
                        <div class="mb-2 sm:mb-0">
                            <p class="text-lg font-bold text-gray-800">${room.name} <span class="text-sm font-normal text-indigo-500">(${room.id})</span></p>
                            <p class="text-sm text-gray-600">${room.type} | R${room.rent}/mo</p>
                            <p class="text-xs text-gray-400 mt-1">${room.details}</p>
                        </div>
                        <div class="flex space-x-3">
                            ${Button('Book', `handleSelectRoom('${room.id}')`, 'green')}
                        </div>
                    </div>
                `).join('')
                : '<p class="text-center py-8 text-gray-500">No rooms currently available for booking.</p>';

            const content = `
                <div class="space-y-4">${roomListHTML}</div>
                <div class="mt-6">${Button('Back to Dashboard', 'setStep(1)', 'gray')}</div>
            `;
            document.getElementById('content-area').innerHTML = Card("Available Rooms for Lease", content);
        };

        // --- Step 3: Booking Choice (Lease or Short-Stay) ---
        const selectBookingType = (type) => {
            bookingType = type;
            if (type === 'Lease') {
                setStep(4);
            }
        };

        const renderBookingChoice = () => {
            if (!selectedRoom) {
                document.getElementById('content-area').innerHTML = Card("Error", '<div class="text-center text-red-500">Error: No room selected.</div>');
                return;
            }

            const content = `
                <p class="text-xl font-medium mb-6 text-gray-700">Choose the tenant agreement type:</p>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div onclick="selectBookingType('Lease')" class="p-6 border-4 border-indigo-100 rounded-xl hover:bg-indigo-50 transition duration-150 cursor-pointer">
                        <h3 class="text-2xl font-bold text-indigo-700 mb-2">Long-Term Lease</h3>
                        <p class="text-gray-600 mb-4">A standard agreement (12+ months). Triggers the full application, e-sign, and payment process.</p>
                        <div class="text-indigo-500 font-semibold">Select for Permanent Tenants &rarr;</div>
                    </div>
                    <div class="p-6 border-4 border-gray-100 rounded-xl opacity-60 cursor-not-allowed">
                        <h3 class="text-2xl font-bold text-gray-700 mb-2">Short-Stay / Vacation Rental</h3>
                        <p class="text-gray-500 mb-4">Used for transient bookings (less than 3 months). (Flow currently not implemented)</p>
                        <div class="text-gray-400 font-semibold">Unavailable for this process</div>
                    </div>
                </div>
                <div class="mt-8">${Button('Back to Room List', 'setStep(2)', 'gray')}</div>
            `;
            document.getElementById('content-area').innerHTML = Card(`Booking for ${selectedRoom.name} (${selectedRoom.id})`, content);
        };

        // --- Step 4: Lease Process Step 1 (Form + ID Upload) ---
        const updateFormData = (e) => {
            const { name, value } = e.target;
            tenantData[name] = value;
            const formValid = tenantData.fullName && tenantData.email && tenantData.idDocumentUrl;
            document.getElementById('next-lease-btn').disabled = !formValid;
        };

        const submitLeaseForm = () => {
            setStep(5);
        };

        const renderLeaseForm = () => {
            if (!selectedRoom) return;

            const content = `
                <p class="text-gray-600 mb-6">Step 1 of 3: Tenant Information & ID Upload</p>
                <div class="space-y-4" oninput="updateFormData(event)">
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-1" for="fullName">Full Name</label>
                        <input type="text" name="fullName" id="fullName" value="${tenantData.fullName}" placeholder="e.g., John Doe" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-indigo-500 focus:border-indigo-500"/>
                    </div>
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-1" for="email">Email Address</label>
                        <input type="email" name="email" id="email" value="${tenantData.email}" placeholder="john.doe@example.com" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-indigo-500 focus:border-indigo-500"/>
                    </div>
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-1" for="phone">Phone Number</label>
                        <input type="tel" name="phone" id="phone" value="${tenantData.phone}" placeholder="(123) 456-7890 (Optional)" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-indigo-500 focus:border-indigo-500"/>
                    </div>
                    <div class="pt-4 border-t mt-4">
                        <label class="block text-sm font-medium text-gray-700 mb-1" for="idDocumentUrl">
                            ID Document Upload <span class="text-red-500">*</span>
                        </label>
                        <input type="file" name="idDocumentUrl" id="idDocumentUrl" value="${tenantData.idDocumentUrl}" class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-indigo-500 focus:border-indigo-500"/>
                        <p class="mt-1 text-xs text-gray-500">Note: In a real system, this would be a secure file upload.</p>
                    </div>
                </div>
                <div class="mt-8 flex justify-between">
                    ${Button('Back', 'setStep(3)', 'gray')}
                    ${Button('Next: E-Sign Agreement', 'submitLeaseForm()', 'indigo', !(tenantData.fullName && tenantData.email && tenantData.idDocumentUrl), 'next-lease-btn')}
                </div>
            `;
            document.getElementById('content-area').innerHTML = Card(`New Lease Application: ${selectedRoom.name} (${selectedRoom.id})`, content);
        };

        // --- Step 5: E-Sign Page ---
        const updateSignatureState = () => {
            const signatureInput = document.getElementById('signature');
            const agreementCheck = document.getElementById('agreementCheck');
            const signature = signatureInput.value;
            const agreed = agreementCheck.checked;

            const nameDisplay = document.getElementById('signature-name-display');
            nameDisplay.textContent = signature || '[Name]';

            const signBtn = document.getElementById('esign-btn');
            signBtn.disabled = !(agreed && signature.trim());
        };

        const eSignAgreement = () => {
            isSigned = true;
            setStep(6);
        };

        const renderESignPage = () => {
            if (!selectedRoom || !tenantData.fullName) return;

            const agreementText = getMockLeaseAgreement(selectedRoom, tenantData);
            const initialSignature = tenantData.fullName;

            const content = `
                <p class="text-gray-600 mb-6">Step 2 of 3: Review and Sign Document</p>
                <div class="border border-gray-300 bg-gray-50 p-4 rounded-lg h-64 overflow-y-scroll text-sm font-mono whitespace-pre-wrap shadow-inner">
                    ${agreementText}
                </div>

                <div class="mt-6 p-4 border rounded-lg bg-yellow-50">
                    <label class="block text-sm font-medium text-gray-700 mb-1" for="signature">Full Name for E-Signature</label>
                    <input
                        type="text"
                        id="signature"
                        value="${initialSignature}"
                        oninput="updateSignatureState()"
                        placeholder="Type your full legal name"
                        class="w-full px-4 py-2 border border-gray-300 rounded-lg font-bold"
                    />

                    <div class="mt-4 flex items-start">
                        <input
                            type="checkbox"
                            id="agreementCheck"
                            onchange="updateSignatureState()"
                            class="h-4 w-4 text-indigo-600 border-gray-300 rounded mt-1"
                        />
                        <label for="agreementCheck" class="ml-3 text-sm text-gray-600">
                            I, <span id="signature-name-display" class="font-bold">${initialSignature}</span>, agree to the terms and conditions outlined above, and this digital signature is legally binding.
                        </label>
                    </div>
                </div>

                <div class="mt-8 flex justify-between">
                    ${Button('Back', 'setStep(4)', 'gray')}
                    ${Button('E-Sign Agreement & Proceed to Payment', 'eSignAgreement()', 'indigo', true, 'esign-btn')}
                </div>
            `;
            document.getElementById('content-area').innerHTML = Card("E-Sign Lease Agreement", content);
            // Initial call to set button state correctly
            updateSignatureState();
        };

        // --- Step 6: Payment Page ---
        const completePayment = () => {
            // Simulate payment processing delay
            const paymentBtn = document.getElementById('payment-btn');
            paymentBtn.disabled = true;
            paymentBtn.innerHTML = 'Processing...';

            setTimeout(() => {
                paymentBtn.innerHTML = 'Complete Payment';
                setStep(7);
            }, 1500);
        };

        const renderPaymentPage = () => {
            if (!selectedRoom) return;

            const totalDue = selectedRoom.rent * 2;
            const finalTotal = totalDue + 50;

            const paymentBreakdown = [
                { label: 'First Month Rent', amount: selectedRoom.rent },
                { label: 'Security Deposit (1x Rent)', amount: selectedRoom.rent },
                { label: 'Admin Fee', amount: 50 }
            ];

            let breakdownHTML = paymentBreakdown.map(item => InfoBox(item.label, `R${item.amount}.00`)).join('');

            const content = `
                <p class="text-gray-600 mb-6">Step 3 of 3: Finalize Payment</p>

                <div class="max-w-md mx-auto p-6 bg-white border border-indigo-200 rounded-xl shadow-lg">
                    <h3 class="text-xl font-bold text-indigo-700 mb-4">Summary of Charges</h3>
                    ${breakdownHTML}

                    <div class="mt-4 pt-4 border-t border-indigo-200 flex justify-between items-center">
                        <span class="text-xl font-bold text-gray-800">TOTAL DUE TODAY:</span>
                        <span class="text-3xl font-extrabold text-green-600">R${finalTotal}.00</span>
                    </div>

                    <div class="mt-6 space-y-3">
                        <input type="text" placeholder="Card Number (4242...)" class="w-full px-4 py-2 border border-gray-300 rounded-lg"/>
                        <div class="flex space-x-3">
                            <input type="text" placeholder="MM/YY" class="w-1/2 px-4 py-2 border border-gray-300 rounded-lg"/>
                            <input type="text" placeholder="CVC" class="w-1/2 px-4 py-2 border border-gray-300 rounded-lg"/>
                        </div>
                    </div>
                </div>

                <div class="mt-8 flex justify-between">
                    ${Button('Back', 'setStep(5)', 'gray')}
                    ${Button(`Complete Payment (R${finalTotal}.00)`, 'completePayment()', 'indigo', false, 'payment-btn')}
                </div>
            `;
            document.getElementById('content-area').innerHTML = Card("Secure Payment Terminal", content);
        };

        // --- Step 7: Confirmation Page ---
        const renderConfirmationPage = () => {
            if (!selectedRoom || !tenantData.fullName) return;

            const mockUsername = tenantData.email.split('@')[0];
            const mockPassword = 'TempPass!2025';
            const link = 'https://tenant-portal.example.com';

            const content = `
                <div class="text-center p-8 bg-green-50 rounded-xl">
                    <svg class="w-16 h-16 mx-auto text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                    </svg>
                    <h3 class="text-3xl font-bold text-green-700 mt-4">Success!</h3>
                    <p class="text-xl text-gray-700 mt-2">The lease for ${selectedRoom.name} is complete.</p>
                </div>

                <div class="mt-8 p-6 border rounded-lg bg-white shadow-md">
                    <h4 class="text-lg font-semibold text-gray-800 mb-4">Details Sent to Tenant (${tenantData.fullName})</h4>
                    <p class="text-gray-600 mb-4">The following information has been automatically emailed to <span class="font-mono text-indigo-600">${tenantData.email}</span>.</p>

                    ${InfoBox("Tenant Portal Link", `<a href="${link}" target="_blank" rel="noopener noreferrer" class="text-indigo-600 hover:underline">${link}</a>`)}
                    ${InfoBox("Username", `<span class="font-mono">${mockUsername}</span>`)}
                    ${InfoBox("Temporary Password", `<span class="font-mono">${mockPassword}</span>`)}
                    ${InfoBox("Lease Agreement PDF", `<span class="text-green-600">Attached to Email</span>`)}
                </div>

                <div class="mt-8">
                    ${Button('Start New Application', 'setStep(1)', 'indigo')}
                </div>
            `;
            document.getElementById('content-area').innerHTML = Card("Process Complete: Lease Initiated", content);
        };


        // --- Main Render Function ---

        const renderProgressBar = () => {
            const container = document.getElementById('progress-bar-container');
            let labelsHTML = flowSteps.map((label, index) => {
                const stepNum = index + 1;
                const activeClass = stepNum <= currentStep ? 'text-indigo-600' : 'text-gray-400';
                return `<div class="text-center transition duration-300 ${activeClass}">${stepNum}. ${label.split(' ')[0]}</div>`;
            }).join('');

            const progressWidth = (currentStep / flowSteps.length) * 100;

            const progressHTML = `
                <div class="flex justify-between items-center text-xs font-semibold text-gray-600 mb-2">
                    ${labelsHTML}
                </div>
                <div class="h-2 bg-gray-300 rounded-full overflow-hidden">
                    <div style="width: ${progressWidth}%" class="h-full bg-indigo-600 transition-all duration-500 ease-in-out"></div>
                </div>
            `;
            container.innerHTML = progressHTML;
        };

        const renderFooter = () => {
             document.getElementById('footer-info').innerHTML = `<p>Current Step: ${currentStep} - ${flowSteps[currentStep - 1]}</p>`;
        }


        const renderStep = () => {
            renderProgressBar();
            renderFooter();

            switch (currentStep) {
                case 1:
                    renderDashboard();
                    break;
                case 2:
                    renderRoomList();
                    break;
                case 3:
                    renderBookingChoice();
                    break;
                case 4:
                    renderLeaseForm();
                    break;
                case 5:
                    renderESignPage();
                    break;
                case 6:
                    renderPaymentPage();
                    break;
                case 7:
                    renderConfirmationPage();
                    break;
                default:
                    document.getElementById('content-area').innerHTML = Card("Error", `
                        <p class="text-red-600">An unexpected error occurred. Please refresh the page.</p>
                        ${Button('Go to Dashboard', 'setStep(1)', 'indigo')}
                    `);
                    break;
            }
        };

        // Initialize the application on load
        window.onload = renderStep;
    