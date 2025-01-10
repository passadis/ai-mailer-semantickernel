import React, { useState, useEffect } from 'react';
import logo from './logo1.png';

const PinAuth = ({ children }) => {
  const [pin, setPin] = useState('');
  const [isAuthorized, setIsAuthorized] = useState(false);
  const [error, setError] = useState('');
  const [attempts, setAttempts] = useState(0);
  const maxAttempts = 3;
  const cooldownTime = 300000; // 5 minutes in milliseconds

  const backendBaseUrl = process.env.REACT_APP_API_URL;

  useEffect(() => {
    // Check if user was previously authenticated
    const authStatus = sessionStorage.getItem('isAuthorized');
    if (authStatus === 'true') {
      setIsAuthorized(true);
    }
  }, []);

  const handlePinChange = (e) => {
    setPin(e.target.value);
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Check if user has exceeded maximum attempts
    if (attempts >= maxAttempts) {
      const lastAttemptTime = parseInt(sessionStorage.getItem('lastAttemptTime') || '0');
      const currentTime = Date.now();
      
      if (currentTime - lastAttemptTime < cooldownTime) {
        const remainingTime = Math.ceil((cooldownTime - (currentTime - lastAttemptTime)) / 60000);
        setError(`Too many attempts. Please try again in ${remainingTime} minutes.`);
        return;
      } else {
        // Reset attempts after cooldown
        setAttempts(0);
        sessionStorage.removeItem('lastAttemptTime');
      }
    }

    try {
      // Replace this with your actual API call to validate PIN
      const response = await fetch(`${backendBaseUrl}/ValidatePin/validate-pin`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ pin }),
      });

      if (response.ok) {
        setIsAuthorized(true);
        sessionStorage.setItem('isAuthorized', 'true');
        setError('');
      } else {
        setAttempts((prev) => prev + 1);
        sessionStorage.setItem('lastAttemptTime', Date.now().toString());
        setError('Invalid PIN. Please try again.');
        
        if (attempts + 1 >= maxAttempts) {
          setError(`Too many attempts. Please try again in 5 minutes.`);
        }
      }
    } catch (error) {
      setError('An error occurred. Please try again.');
    }
  };

  if (isAuthorized) {
    return <>{children}</>;
  }

  return (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      minHeight: '100vh',
      backgroundColor: '#f3f4f6'
    }}>
      <div style={{
        padding: '2rem',
        backgroundColor: 'white',
        borderRadius: '8px',
        boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
        width: '100%',
        maxWidth: '400px'
      }}>
        <img src={logo} alt="Logo" style={{ display: 'block', margin: '0 auto 1rem', width: '100px' }} />
        <h2 style={{
          marginBottom: '1.5rem',
          textAlign: 'center',
          fontSize: '1.5rem',
          fontWeight: 'bold'
        }}>
          Enter PIN to Access
        </h2>
        <form onSubmit={handleSubmit} style={{display: 'flex', flexDirection: 'column', gap: '1rem'}}>
          <input
            type="password"
            value={pin}
            onChange={handlePinChange}
            placeholder="Enter PIN"
            style={{
              padding: '0.5rem',
              border: '1px solid #d1d5db',
              borderRadius: '4px',
              width: '100%'
            }}
            maxLength={12}
            disabled={attempts >= maxAttempts}
          />
          {error && (
            <div style={{
              padding: '0.5rem',
              backgroundColor: '#fee2e2',
              color: '#dc2626',
              borderRadius: '4px',
              fontSize: '0.875rem'
            }}>
              {error}
            </div>
          )}
          <button
            type="submit"
            disabled={!pin || attempts >= maxAttempts}
            style={{
              padding: '0.5rem',
              backgroundColor: !pin || attempts >= maxAttempts ? '#d1d5db' : '#3b82f6',
              color: 'white',
              border: 'none',
              borderRadius: '4px',
              cursor: !pin || attempts >= maxAttempts ? 'not-allowed' : 'pointer'
            }}
          >
            Submit
          </button>
        </form>
      </div>
    </div>
  );
};

export default PinAuth;
