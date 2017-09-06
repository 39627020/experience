package com.jzd1997.hellospring.domain;

import org.springframework.cache.annotation.CacheConfig;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
@CacheConfig(cacheNames = "users")
public interface UserRepository extends JpaRepository<User, Long> {
	@Cacheable
    User findByName(String name);
	@Cacheable
    User findByNameAndAge(String name, Integer age);
	@Cacheable
    @Query("from User u where u.name=:name")
    User findUser(@Param("name") String name);


}

