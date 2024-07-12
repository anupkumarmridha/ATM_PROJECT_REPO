1. **Card Validation:**
   - User enters card details (card number, CVV, expiry date).
   - Validate the card details against industry standards (e.g., Luhn algorithm for card number).
   - If validation fails, prompt the user to re-enter correct details.

2. **PIN Validation:**
   - Once card validation succeeds, prompt the user to enter their PIN.
   - Validate the PIN against the stored PIN for the user.
   - If validation fails, notify the user and allow them to retry.

3. **Display Balance and Menu:**
   - After successful PIN validation, display the user's account balance.
   - Present a menu with options: "Deposit" and "Withdraw."

4. **Withdrawal Validation:**
   - For withdrawal:
     - User enters the withdrawal amount.
     - Validate the withdrawal amount:
       - Ensure the account balance is sufficient.
       - Limit withdrawal amount to $10,000 (if exceeded, notify the user).

5. **Deposit Validation:**
   - For deposit:
     - User enters the deposit amount.
     - Validate the deposit amount:
       - Limit deposit amount to $20,000 (if exceeded, notify the user).

6. **OTP Generation and Sending:**
   - After successful amount validation (withdrawal or deposit), generate an OTP.
   - Send the OTP to the user's registered email address.

7. **OTP Validation:**
   - Prompt the user to enter the received OTP.
   - Validate the entered OTP against the generated OTP.
   - If validation fails, allow the user to retry.

8. **Update Account and Transaction Completion:**
   - If OTP validation succeeds:
     - Update the user's account balance (debit for withdrawal, credit for deposit).
     - Mark the transaction as completed.
     - Send a transaction completion email to the user.

9. **Transaction Emails:**
   - For withdrawal:
     - Send a debited transaction email with details (amount, timestamp, etc.).
   - For deposit:
     - Send a credited transaction email with details.
