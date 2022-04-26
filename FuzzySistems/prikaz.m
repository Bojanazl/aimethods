%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%
%       PRIKAZ REZULTATA SIMULACIJE
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% CRTANJE ZELJENE BRZINE I STVARNE BRZINE OBRTANJA
figure(1)

subplot(2,1,1)

plot(vrijeme, zeljena_brzina,'r', vrijeme, stvarna_brzina,'b')
grid on
xlabel('vrijeme [s]')
ylabel('BRZINA [r.j.]')
legend('ZELJENA BRZINA','STVARNA BRZINA')

% CRTANJE IZLAZA REGULATORA (NAPON [r.j.])


subplot(2,1,2)

plot(vrijeme, napon)
grid on
xlabel('vrijeme [s]')
ylabel('NAPON [r.j.]')


% Dozvoljeno je na ovaj nacin prikazivati i druge velicine u izvještaju!