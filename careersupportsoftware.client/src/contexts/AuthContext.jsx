// src/contexts/AuthContext.jsx
import React, { createContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const AuthContext = createContext({
    user: null,
    login: async () => { },
    register: async () => { },
    logout: () => { }
});

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const navigate = useNavigate();

    // Helper to read token & email from localStorage
    useEffect(() => {
        const token = localStorage.getItem('token');
        const email = localStorage.getItem('email');
        if (token && email) {
            setUser({ email });
        }
    }, []);

    // POST /api/auth/login
    const login = async (email, password) => {
        const res = await fetch('/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        if (!res.ok) {
            const text = await res.text();
            throw new Error(text || 'Login failed');
        }
        const data = await res.json();
        localStorage.setItem('token', data.token);
        localStorage.setItem('email', email);
        setUser({ email });
    };

    // POST /api/auth/register
    const register = async (email, password) => {
        const res = await fetch('/api/auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        if (!res.ok) {
            // Try to parse a JSON error, otherwise fallback to text
            let err;
            try {
                const json = await res.json();
                err = json?.message || JSON.stringify(json);
            } catch {
                err = await res.text();
            }
            throw new Error(err || 'Registration failed');
        }
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('email');
        setUser(null);
        navigate('/login', { replace: true });
    };

    return (
        <AuthContext.Provider value={{ user, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
}
